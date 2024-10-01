using System;
using System.Linq;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
using nitou.BachProcessor;
using System.Collections.Generic;

namespace nitou.LevelActors.Check {
    using nitou.LevelActors.Core;
    using nitou.LevelActors.Interfaces.Core;
    using nitou.LevelActors.Interfaces.Components;

    /// <summary>
    /// 接地状態の検出用コンポーネント．
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GroundCheck : MonoBehaviour,
        IEarlyUpdateComponent {

        [Title("Parameters")]

        /// <summary>
        /// The maximum distance at which the character can be recognized as being on the ground.
        /// This distance is used for ambiguous ground detection.
        /// </summary>
        [PropertyRange(0, 2f)]
        [SerializeField, Indent] float _ambiguousDistance = 0.2f;

        /// <summary>
        /// The distance at which the character can be recognized as being on the ground.
        /// This distance is used for strict ground detection.
        /// </summary>
        [PropertyRange(0, 0.5f)]
        [SerializeField, Indent] float _preciseDistance = 0.02f;

        /// <summary>
        /// This is the maximum slope at which the ground is recognized as "ground".
        /// If the slope of the nearest ground is greater than this angle, IsGround will be set to false.
        /// </summary>
        [PropertyRange(0, 90)]
        [SerializeField, Indent] int _maxSlope = 60;

        /// <summary>
        /// 
        /// </summary>
        public IObservable<GameObject> OnGrounObjectChanged => _onGroundObjectChandedSubject;
        private readonly Subject<GameObject> _onGroundObjectChandedSubject = new();

        // references
        private ActorBody _actorBody;
        private ITransform _transform;

        // 内部処理用
        private readonly RaycastHit[] _hits = new RaycastHit[MAX_COLLISION_SIZE];
        private RaycastHit _groundCheckHit;

        // 定数
        private const int MAX_COLLISION_SIZE = 5;


        /// ----------------------------------------------------------------------------
        // Property

        int IEarlyUpdateComponent.Order => Order.Check;

        /// <summary>
        /// This checks for ground detection.
        /// Returns true if there is a collider within range.
        /// This calculation result used when you want to avoid small fluctuations in ground detection, such as for between Animator state.
        /// </summary>
        [ShowInInspector, ReadOnly]
        public bool IsOnGround { get; private set; }

        /// <summary>
        /// Returns true if the character is in contact with the ground.
        /// This property is stricter than IsGrounded. This property is mainly used for positioning the character.
        /// </summary>
        public bool IsFirmlyOnGround { get; private set; }

        /// <summary>
        /// 現在フレームで地面オブジェクトが切り替わったかどうか
        /// </summary>
        public bool IsChangeGroundObject { get; private set; } = false;


        /// <summary>
        /// 地面オブジェクト. (※非接地時はnull)
        /// </summary>
        public GameObject GroundObject { get; private set; } = null;

        /// <summary>
        /// Returns the current ground collider. If the object is not grounded, returns null.
        /// </summary>
        public Collider GroundCollider { get; private set; } = null;

        /// <summary>
        /// Get the distance to the ground.
        /// If not grounded, return the maximum distance.
        /// </summary>
        public float DistanceFromGround { get; private set; }

        /// <summary>
        /// Returns the orientation of the ground surface. If not grounded, it returns Vector3.Up.
        /// </summary>
        public Vector3 GroundSurfaceNormal { get; private set; } = Vector3.up;

        /// <summary>
        /// Get the grounded position.
        /// If not grounded, return Vector3.Zero.
        /// </summary>
        public Vector3 GroundContactPoint { get; private set; }


        /// ----------------------------------------------------------------------------
        // Lifecycle Events

        private void Awake() {
            GatherComponents();
        }

        private void OnDestroy() {
            _onGroundObjectChandedSubject.Dispose();
        }

        void IEarlyUpdateComponent.OnUpdate(float deltaTime) {

            using var _ = new ProfilerScope("Ground Check");
            var preGroundObject = GroundObject;
            var offset = _actorBody.Radius * 2;
            var origin = new Vector3(0, offset, 0) + (_transform?.Position ?? transform.position);
            var groundCheckDistance = _ambiguousDistance + offset;

            // Perform ground detection while ignoring the character's own collider.
            var groundCheckCount = Physics.SphereCastNonAlloc(
                origin, _actorBody.Radius, Vector3.down, _hits,
                groundCheckDistance,
                _actorBody.EnvironmentLayer, QueryTriggerInteraction.Ignore);
            var isHit = ClosestHit(_hits, groundCheckCount, groundCheckDistance, out _groundCheckHit);

            // fill the properties of the component based on the information of the ground.
            if (isHit) {
                var inLimitAngle = Vector3.Angle(Vector3.up, _groundCheckHit.normal) < _maxSlope;

                DistanceFromGround = _groundCheckHit.distance - (offset - _actorBody.Radius);
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
                using var invokeProfile = new ProfilerScope("Ground Check.Invoke");
                _onGroundObjectChandedSubject.OnNext(GroundObject);
            }

        }


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// Performs a Raycast that ignores the Collider attached to the character.
        /// This API is used, for example, to detect a step in front of the character.
        /// </summary>
        public bool Raycast(Vector3 position, float distance, out RaycastHit hit) {
            var groundCheckCount = Physics.RaycastNonAlloc(position, Vector3.down, _hits, distance,
                _actorBody.EnvironmentLayer, QueryTriggerInteraction.Ignore);

            // 最も近いオブジェクト
            return _actorBody.ClosestHit(_hits, groundCheckCount, distance, out hit);
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private void GatherComponents() {
            if(_transform == null) {
                _transform = gameObject.GetComponentInParent<ITransform>();
            }

            if(_actorBody == null) {
                _actorBody = gameObject.GetComponentInParent<ActorBody>();
            }
        }

        private bool ClosestHit(IReadOnlyList<RaycastHit> hits, int count, float maxDistance, out RaycastHit closestHit) {

            var min = maxDistance;
            closestHit = new RaycastHit();
            var isHit = false;

            foreach (var hit in hits.Take(count)) {
                var isOverOriginHeight = (hit.distance == 0);
                if (isOverOriginHeight || hit.distance > min || _actorBody.IsOwnCollider(hit.collider) || hit.collider == null)
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
            _ambiguousDistance = 0.2f;
        }

        private void OnDrawGizmosSelected() {

            void DrawHitRangeGizmos(Vector3 startPosition, Vector3 endPosition) {
                var leftOffset = new Vector3(_actorBody.Radius, 0, 0);
                var rightOffset = new Vector3(-_actorBody.Radius, 0, 0);
                var forwardOffset = new Vector3(0, 0, _actorBody.Radius);
                var backOffset = new Vector3(0, 0, -_actorBody.Radius);

                Gizmos.DrawLine(startPosition + leftOffset, endPosition + leftOffset);
                Gizmos.DrawLine(startPosition + rightOffset, endPosition + rightOffset);
                Gizmos.DrawLine(startPosition + forwardOffset, endPosition + forwardOffset);
                Gizmos.DrawLine(startPosition + backOffset, endPosition + backOffset);
                Gizmos_.DrawSphere(startPosition, _actorBody.Radius, Color.yellow);
                Gizmos_.DrawSphere(endPosition, _actorBody.Radius, Color.yellow);
            }

            if (_actorBody == null) {
                _actorBody = gameObject.GetComponentInParent<ActorBody>();
            }

            var position = transform.position;
            var offset = _actorBody.Height.Half();


            if (Application.isPlaying) {
                Gizmos.color = IsOnGround ? Color.red : Gizmos.color;
                Gizmos.color = IsFirmlyOnGround ? Color.blue : Gizmos.color;

                var topPosition = new Vector3 { y = _actorBody.Radius - _preciseDistance };
                var bottomPosition = IsOnGround
                    ? new Vector3 { y = _actorBody.Radius - DistanceFromGround }
                    : new Vector3 { y = _actorBody.Radius - _ambiguousDistance };

                DrawHitRangeGizmos(position + topPosition, position + bottomPosition);

                if (IsOnGround) {
                    Gizmos_.DrawCollider(GroundCollider, Color.green);
                    Gizmos.color = Color.white;
                    Gizmos.DrawSphere(_groundCheckHit.point, 0.1f);
                    Gizmos.DrawRay(_groundCheckHit.point, GroundSurfaceNormal * offset);
                }
            } else {
                var topPosition = new Vector3 { y = _actorBody.Radius - _preciseDistance };
                var bottomPosition = new Vector3 { y = _actorBody.Radius - _ambiguousDistance };
                DrawHitRangeGizmos(position + topPosition, position + bottomPosition);
            }
        }
#endif
    }

}
