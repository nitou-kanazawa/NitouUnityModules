using UnityEngine;
using Sirenix.OdinInspector;
using nitou.Attributes;

namespace nitou.LevelActors.Controller.Control{

    using Controller.Interfaces.Core;
    using Controller.Interfaces.Components;
    using Controller.Core;
    using Controller.Shared;

    /// <summary>
    /// 
    /// </summary>
    [AddComponentMenu(MenuList.MenuControl + nameof(MoveControl))]
    [DisallowMultipleComponent]
    [RequireInterface(typeof(IGroundContact))]
    public sealed class MoveControl : MonoBehaviour,
        IMove,
        ITurn,
        IPriorityLifecycle<ITurn>,
        IUpdateComponent {

        [Title("Movement settings")]

        /// <summary>
        /// アクターの最大移動速度．
        /// </summary>
        [SerializeField, Indent] float _moveSpeed = 4;

        /// <summary>
        /// アクター回転速度．（Priorityが高いときのみ適用）
        /// </summary>
        [PropertyRange(-1, 50)]
        [SerializeField, Indent] int _turnSpeed = 15;

        /// <summary>
        /// ブレーキ力．
        /// </summary>
        [SerializeField, Indent] float _brakePower = 12;

        /// <summary>
        /// 加速度．
        /// </summary>
        [SerializeField, Indent] float _accelerator = 6;

        /// <summary>
        /// 移動可能な傾斜角度。
        /// 地形の角度がこの値以下の場合、その地形に沿って移動する．
        /// </summary>
        [SerializeField, Indent] float _angle = 45;

        /// <summary>
        /// 移動可能方向を制限するための任意軸．値はグローバル座標系．
        /// Vector3.zero以外が指定されると、アクターはその軸方向のみに移動する．
        /// </summary>
        [SerializeField, Indent] Vector3 _lockAxis = Vector3.zero;


        [Title("Priority Settings")]

        /// <summary>
        /// Determines if MoveControl is used to move the character.
        /// If a higher value than other Priority is set, this component is used.
        /// </summary>
        [SerializeField, Indent] int _movePriority = 1;

        /// <summary>
        /// Threshold to determine if the character is in motion.
        /// If the value falls below this threshold, set <see cref="IsMove"/> to False.
        /// </summary>
        [Range(0, 1)]
        [SerializeField, Indent] float _moveStopThreshold = 0.2f;

        /// <summary>
        /// Determines if MoveControl is used for character orientation.
        /// If a higher value is set compared to other priorities, this component is used.
        /// </summary>
        [SerializeField, Indent] int _turnPriority = 1;

        /// <summary>
        /// Threshold to determine if the character's orientation has reached.
        /// If it's 0, orientation changes immediately after stopping the movement.
        /// If it's 1, the orientation is updated until it reaches the target orientation.
        /// </summary>
        [Range(0, 1)]
        [SerializeField, Indent] float _turnStopThreshold = 0;

        // references
        private ActorSettings _actorSettings;
        private IBrain _brain;
        private IGroundContact _groundCheck;
        private Transform _transform;

        // state
        private bool _hasGroundCheck;
        private Vector3 _moveDirection = Vector3.forward;
        private float _currentSpeed;
        private Vector2 _inputValue;
        private bool _hasInput;
        private float _yawAngle;
        private bool _isTurning;


        /// ----------------------------------------------------------------------------
        // Properity

        /// <summary>
        /// 処理順序．
        /// </summary>
        int IUpdateComponent.Order => Order.Control;

        /// <summary>
        /// 
        /// </summary>
        public MovementReference MovementReference => _actorSettings.MovementReference;

        /// <summary>
        /// 現在の移動速度．
        /// <see cref="IPriority{IMove}.Priority">優先度</see>/>が0の場合、移動速度も0になる
        /// </summary>
        public float CurrentSpeed {
            get => (_movePriority > 0 || _currentSpeed < _moveStopThreshold) ? _currentSpeed : 0;
            set => _currentSpeed = value;
        }

        /// <summary>
        /// Movement vector in world coordinates
        /// Updated after Update
        /// </summary>
        public Vector3 MoveVelocity { get; private set; }

        /// <summary>
        /// Current Velocity
        /// </summary>
        public Vector3 Velocity {
            get => _moveDirection * _currentSpeed;
            set {
                _moveDirection = value.normalized;
                _currentSpeed = value.magnitude;
            }
        }

        /// <summary>
        /// Character-based movement vector
        /// </summary>
        public Vector3 LocalVelocity => Quaternion.Inverse(_transform.rotation) * MoveVelocity;

        /// <summary>
        /// The direction the character wants to move in world coordinates.
        /// This value multiplied by Speed is the actual amount of movement.
        /// </summary>
        public Vector3 Direction { get; private set; }

        /// <summary>
        /// The direction of movement from the character's perspective
        /// </summary>
        public Vector3 LocalDirection => Quaternion.Inverse(_transform.rotation) * Direction;

        /// <summary>
        /// Delta turn angle from previous frame.
        /// This value does not take TurnSpeed into account.
        /// </summary>
        public float DeltaTurnAngle { get; private set; }

        /// <summary>
        /// Turn speed
        /// </summary>
        public int TurnSpeed {
            get => _turnSpeed;
            set => _turnSpeed = value;
        }

        /// <summary>
        /// Maximum character movement speed
        /// </summary>
        public float MoveSpeed {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        /// <summary>
        /// 移動方向の変化量 (degree)
        /// </summary>
        public float DeltaDirectionAngle => Vector3.SignedAngle(_transform.forward, _moveDirection, Vector3.up);

        /// <summary>.
        /// Character movement priority
        /// If priority is 0, stop moving
        /// </summary>
        public int MovePriority {
            get => _movePriority;
            set {
                IsMove = value != 0;
                _movePriority = value;
            }
        }

        /// <summary>
        /// True if the character's movement axis is restricted.
        /// </summary>
        public bool IsLockAxis => _lockAxis.sqrMagnitude > 0;

        /// <summary>.
        /// True if the character is moving.
        /// Movement is determined based on <see cref="_moveStopThreshold"/>.
        /// </summary>
        public bool IsMove { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        float ITurn.YawAngle => _yawAngle;

        void IPriorityLifecycle<ITurn>.OnUpdateWithHighestPriority(float deltaTime) {
            if (_hasInput) {
                _yawAngle = Vector3.SignedAngle(Vector3.forward, _moveDirection, Vector3.up);
            }
        }

        void IPriorityLifecycle<ITurn>.OnAcquireHighestPriority() {
            if (IsMove == false)
                _yawAngle = _brain.YawAngle;
        }

        void IPriorityLifecycle<ITurn>.OnLoseHighestPriority() { }

        int IPriority<ITurn>.Priority => IsMove || _isTurning ? _turnPriority : 0;
        int IPriority<IMove>.Priority => _currentSpeed > _moveStopThreshold ? _movePriority : 0;


        /// ----------------------------------------------------------------------------
        // Lifecycle Events

        private void Awake() {
            // ActorSettings
            _actorSettings = gameObject.GetComponentInParent<ActorSettings>() ?? throw new System.NullReferenceException(nameof(_actorSettings));

            // Components
            _actorSettings.TryGetComponent<Transform>(out _transform);
            _actorSettings.TryGetComponent<IBrain>(out _brain);
            _hasGroundCheck = _actorSettings.TryGetActorComponent(ActorComponent.Check, out _groundCheck);
        }

        private void OnDestroy() {
            
        }

        void IUpdateComponent.OnUpdate(float dt) {
            using (new ProfilerScope(nameof(MoveControl))) {
                ProcessMove(dt);
                ProcessTurn(dt);
            }
        }


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// Moves according to the stick input.
        /// </summary>
        /// <param name="leftStick">Direction of movement.</param>
        public void Move(Vector2 leftStick) {
            // 入力情報をキャッシュする
            _inputValue = leftStick;
            _hasInput = leftStick.sqrMagnitude > 0;
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private void ProcessMove(float dt) {
            // 移動入力があった場合
            if (_hasInput) {

                var preDirection = _moveDirection;
                MovementReference.UpdateInputData(_inputValue);
                _moveDirection = MovementReference.ModifieredInputVector;


                //var cameraYawRotation = Quaternion.AngleAxis(_actorSettings.CameraTransform.rotation.eulerAngles.y, Vector3.up);
                //var direction = new Vector3(_inputValue.x, 0, _inputValue.y);

                //// Determines direction of movement according to camera orientation
                //_moveDirection = cameraYawRotation * direction.normalized;

                if (IsLockAxis) {
                    var dot = Vector3.Dot(_moveDirection, _lockAxis);
                    _moveDirection = _lockAxis * Mathf.Round(Mathf.Clamp(dot * 100, -1, 1));
                }

                _currentSpeed = Mathf.Lerp(_currentSpeed, _moveSpeed, _accelerator * dt);
                DeltaTurnAngle = Vector3.SignedAngle(preDirection, _moveDirection, Vector3.up);

            }
            // 移動入力がない場合，
            else {

                DeltaTurnAngle = 0;
                _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _brakePower * dt);
                if (_currentSpeed < _moveStopThreshold)
                    _currentSpeed = 0;
            }
        }

        private void ProcessTurn(float dt) {
            // Determines direction of movement according to ground information
            var normal = _hasGroundCheck && _groundCheck.IsOnGround ? _groundCheck.GroundSurfaceNormal : Vector3.up;
            normal = Vector3.Angle(Vector3.up, normal) < _angle ? normal : Vector3.up;
            Direction = Vector3.ProjectOnPlane(_moveDirection, normal);

            //
            MoveVelocity = Direction * _currentSpeed;
            IsMove = _currentSpeed > _moveStopThreshold;
            _isTurning = Vector3.Angle(_transform.forward, _moveDirection) > (1 - _turnStopThreshold) * 360;
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR

        private void OnDrawGizmosSelected() {

            var offset = new Vector3(0, 0.1f, 0);
            var position = transform.position + offset;

            // 移動の制限軸.
            if (IsLockAxis) {
                var size = Vector3.one * 0.2f;
                var p1 = position + _lockAxis * 5;
                var p2 = position - _lockAxis * 5;

                var color = Colors.Green.WithAlpha(0.2f);
                Gizmos_.DrawCube(p1, size, color);
                Gizmos_.DrawCube(p2, size, color);
                Gizmos_.DrawLine(p1, p2, Colors.Green);
            }

            // show line about Move velocities.
            Gizmos_.DrawRay(position, MoveVelocity, Colors.Green);
        }
#endif
    }
}

