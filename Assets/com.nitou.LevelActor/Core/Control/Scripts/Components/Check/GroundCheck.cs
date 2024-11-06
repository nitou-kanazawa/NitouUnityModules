using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;

namespace nitou.LevelActors.Controller.Check {
    using nitou.LevelActors.Controller.Core;
    using nitou.LevelActors.Controller.Interfaces.Core;
    using nitou.LevelActors.Controller.Interfaces.Components;
    using nitou.LevelActors.Controller.Shared;

    /// <summary>
    /// �n�ʂƂ̏Փˌ��o�R���|�[�l���g.�n�ʂ����݂��邩�ǂ�����ڐG���Ă���ʂ̌����Ȃ�,
    /// �ڐG���Ă���I�u�W�F�N�g�Ɋւ�����𔻒f���A�n�ʃI�u�W�F�N�g�̕ω���ʒm����.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu(MenuList.MenuCheck + nameof(GroundCheck))]
    public sealed class GroundCheck : MonoBehaviour,
        IGroundContact,
        IGroundObject,
        IEarlyUpdateComponent {

        [Title("Parameters")]

        /// <summary>
        /// �n�ʂɂ���ƔF�������ő勗���D(��܂��Ȓn�ʌ��o�p�j
        /// </summary>
        [PropertyRange(0, 2f)]
        [SerializeField, Indent] float _ambiguousDistance = 0.2f;

        /// <summary>
        /// �n�ʂɂ���ƔF������鋗���D(�����Ȓn�ʌ��o�p�j
        /// </summary>
        [PropertyRange(0, 0.5f)]
        [SerializeField, Indent] float _preciseDistance = 0.02f;

        /// <summary>
        /// �n�ʂƔF�������ő�X�Ίp�x�D
        /// �ł��߂��n�ʂ̌X�΂����̊p�x�𒴂���ꍇ�AIsGround��false�ɂȂ�
        /// </summary>
        [PropertyRange(0, 90)]
        [SerializeField, Indent] int _maxSlope = 60;

        // references
        private ActorSettings _actorSettings;
        private ITransform _transform;

        // ���������p
        private readonly RaycastHit[] _hits = new RaycastHit[MAX_COLLISION_SIZE];
        private RaycastHit _groundCheckHit;
        private readonly Subject<GameObject> _onGroundObjectChandedSubject = new();

        // �萔
        private const int MAX_COLLISION_SIZE = 5;


        /// ----------------------------------------------------------------------------
        // Property

        /// <summary>
        /// �����I�[�_�[�D
        /// </summary>
        int IEarlyUpdateComponent.Order => Order.Check;

        /// <summary>
        /// �ڒn��Ԃ̑�܂��Ȕ���D
        /// �͈͓��ɃR���C�_�[������ꍇ��true��Ԃ��D
        /// ���̌v�Z���ʂ́A�A�j���[�^�[�̏�ԊԂȂǂŒn�ʌ��o�̔����ȕϓ�����������Ƃ��Ɏg�p�����D
        /// </summary>
        public bool IsOnGround { get; private set; }

        /// <summary>
        /// �ڒn��Ԃ̌����Ȕ���D
        /// �A�N�^�[�̈ʒu���߂Ɏg�p�����D
        /// </summary>
        public bool IsFirmlyOnGround { get; private set; }


        /// <summary>
        /// �n�ʃI�u�W�F�N�g. 
        /// �ڒn���Ă��Ȃ��ꍇ��null��Ԃ��D
        /// </summary>
        public GameObject GroundObject { get; private set; } = null;

        /// <summary>
        /// ���݂̒n�ʃR���C�_�[�D
        /// �ڒn���Ă��Ȃ��ꍇ��null��Ԃ��D
        /// </summary>
        public Collider GroundCollider { get; private set; } = null;

        /// <summary>
        /// �n�ʂ܂ł̋����D
        /// �ڒn���Ă��Ȃ��ꍇ�͍ő勗����Ԃ��D
        /// </summary>
        public float DistanceFromGround { get; private set; }

        /// <summary>
        /// �n�ʂ̖@���x�N�g���D
        /// �ڒn���Ă��Ȃ��ꍇ��Vector3.Up��Ԃ��D
        /// </summary>
        public Vector3 GroundSurfaceNormal { get; private set; } = Vector3.up;

        /// <summary>
        /// �n�ʂƂ̐ڒn�ʒu�D
        /// �ڒn���Ă��Ȃ��ꍇ��Vector3.Zero��Ԃ��D
        /// </summary>
        public Vector3 GroundContactPoint { get; private set; }

        /// <summary>
        /// ���݃t���[���Œn�ʃI�u�W�F�N�g���؂�ւ�������ǂ���
        /// </summary>
        public bool IsChangeGroundObject { get; private set; } = false;


        /// <summary>
        /// �n�ʃI�u�W�F�N�g���ω������Ƃ��̃C�x���g�ʒm
        /// </summary>
        public IObservable<GameObject> OnGrounObjectChanged => _onGroundObjectChandedSubject;


        /// ----------------------------------------------------------------------------
        // Lifecycle Events

        private void Awake() {
            GatherComponents();
        }

        private void OnDestroy() {
            _onGroundObjectChandedSubject.Dispose();
        }

        void IEarlyUpdateComponent.OnUpdate(float deltaTime) {
            using var _ = new ProfilerScope(nameof(GroundCheck));

            var preGroundObject = GroundObject;
            var offset = _actorSettings.Radius * 2;
            var origin = new Vector3(0, offset, 0) + (_transform?.Position ?? transform.position);
            var groundCheckDistance = _ambiguousDistance + offset;

            // Perform ground detection while ignoring the character's own collider.
            var groundCheckCount = Physics.SphereCastNonAlloc(
                origin, _actorSettings.Radius, Vector3.down, _hits,
                groundCheckDistance,
                _actorSettings.EnvironmentLayer, QueryTriggerInteraction.Ignore);

            var isHit = ClosestHit(_hits, groundCheckCount, groundCheckDistance, out _groundCheckHit);

            // fill the properties of the component based on the information of the ground.
            if (isHit) {
                var inLimitAngle = Vector3.Angle(Vector3.up, _groundCheckHit.normal) < _maxSlope;

                DistanceFromGround = _groundCheckHit.distance - (offset - _actorSettings.Radius);
                IsOnGround = DistanceFromGround < _ambiguousDistance;
                IsFirmlyOnGround = DistanceFromGround <= _preciseDistance && inLimitAngle;
                GroundContactPoint = _groundCheckHit.point;
                GroundSurfaceNormal = _groundCheckHit.normal;
                GroundCollider = _groundCheckHit.collider;
                GroundObject = IsOnGround ? _groundCheckHit.collider.gameObject : null;

            } else {
                DistanceFromGround = _ambiguousDistance;
                IsOnGround = false;
                IsFirmlyOnGround = false;
                GroundContactPoint = Vector3.zero;
                GroundSurfaceNormal = Vector3.zero;
                GroundCollider = null;
                GroundObject = null;
            }

            // If the object has changed, invoke _onChangeGroundObject.
            IsChangeGroundObject = preGroundObject != GroundObject;
            if (IsChangeGroundObject) {
                using var invokeProfile = new ProfilerScope($"{nameof(GroundCheck)}.Invoke");
                _onGroundObjectChandedSubject.OnNext(GroundObject);
            }

        }


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// �A�N�^�[���g�ɃA�^�b�`����Ă���R���C�_�[�𖳎�����Raycast�����s����D
        /// ����API�͗Ⴆ�΃L�����N�^�[�̑O���ɂ���i�������o���邽�߂Ɏg�p�����D
        /// </summary>
        /// <param name="position">�J�n�ʒu�B</param>
        /// <param name="distance">���C�͈̔́B</param>
        /// <param name="hit">�q�b�g����RaycastHit��Ԃ��܂��B</param>
        /// <returns>�R���C�_�[�Ƀq�b�g�����ꍇ��true��Ԃ��܂��B</returns>
        public bool Raycast(Vector3 position, float distance, out RaycastHit hit) {
            var groundCheckCount = Physics.RaycastNonAlloc(position, Vector3.down, _hits, distance,
                _actorSettings.EnvironmentLayer, QueryTriggerInteraction.Ignore);

            // �ł��߂��I�u�W�F�N�g
            return _actorSettings.ClosestHit(_hits, groundCheckCount, distance, out hit);
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private void GatherComponents() {
            _actorSettings = GetComponentInParent<ActorSettings>() ?? throw new System.NullReferenceException(nameof(_actorSettings));

            // Components
            _actorSettings.TryGetComponent(out _transform);
        }

        private bool ClosestHit(IReadOnlyList<RaycastHit> hits, int count, float maxDistance, out RaycastHit closestHit) {

            var min = maxDistance;
            closestHit = new RaycastHit();
            var isHit = false;

            foreach (var hit in hits.Take(count)) {
                var isOverOriginHeight = (hit.distance == 0);
                if (isOverOriginHeight || hit.distance > min || _actorSettings.IsOwnCollider(hit.collider) || hit.collider == null)
                    continue;

                min = hit.distance;
                closestHit = hit;
                isHit = true;
            }

            return isHit;
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void Reset() {
            _ambiguousDistance = GetComponentInParent<ActorSettings>().Height * 0.35f;
        }

        private void OnDrawGizmosSelected() {

            void DrawHitRangeGizmos(Vector3 startPosition, Vector3 endPosition) {
                var leftOffset = new Vector3(_actorSettings.Radius, 0, 0);
                var rightOffset = new Vector3(-_actorSettings.Radius, 0, 0);
                var forwardOffset = new Vector3(0, 0, _actorSettings.Radius);
                var backOffset = new Vector3(0, 0, -_actorSettings.Radius);

                Gizmos.DrawLine(startPosition + leftOffset, endPosition + leftOffset);
                Gizmos.DrawLine(startPosition + rightOffset, endPosition + rightOffset);
                Gizmos.DrawLine(startPosition + forwardOffset, endPosition + forwardOffset);
                Gizmos.DrawLine(startPosition + backOffset, endPosition + backOffset);
                Gizmos_.DrawSphere(startPosition, _actorSettings.Radius, Color.yellow);
                Gizmos_.DrawSphere(endPosition, _actorSettings.Radius, Color.yellow);
            }

            if (_actorSettings == null) {
                _actorSettings = gameObject.GetComponentInParent<ActorSettings>();
            }

            var position = transform.position;
            var offset = _actorSettings.Height.Half();


            if (Application.isPlaying) {
                Gizmos.color = IsOnGround ? Color.red : Gizmos.color;
                Gizmos.color = IsFirmlyOnGround ? Color.blue : Gizmos.color;

                var topPosition = new Vector3 { y = _actorSettings.Radius - _preciseDistance };
                var bottomPosition = IsOnGround
                    ? new Vector3 { y = _actorSettings.Radius - DistanceFromGround }
                    : new Vector3 { y = _actorSettings.Radius - _ambiguousDistance };

                DrawHitRangeGizmos(position + topPosition, position + bottomPosition);

                if (IsOnGround) {
                    Gizmos_.DrawCollider(GroundCollider, Color.green);
                    Gizmos.color = Color.white;
                    Gizmos.DrawSphere(_groundCheckHit.point, 0.1f);
                    Gizmos.DrawRay(_groundCheckHit.point, GroundSurfaceNormal * offset);
                }
            } else {
                var topPosition = new Vector3 { y = _actorSettings.Radius - _preciseDistance };
                var bottomPosition = new Vector3 { y = _actorSettings.Radius - _ambiguousDistance };
                DrawHitRangeGizmos(position + topPosition, position + bottomPosition);
            }
        }
#endif
    }

}
