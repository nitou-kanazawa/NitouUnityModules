using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelActors.Control{
    using nitou.LevelActors.Interfaces.Core;
    using nitou.LevelActors.Interfaces.Components;
    using nitou.LevelActors.Core;

    [DisallowMultipleComponent]
    public sealed class MoveControl : MonoBehaviour,
        ITurn,
        IMove,
        IPriorityLifecycle<ITurn>,
        IUpdateComponent {

        [Title("Movement settings")]

        /// <summary>
        /// Maximum movement speed of the character.
        /// </summary>
        [SerializeField, Indent] float _moveSpeed = 4;

        /// <summary>
        /// Rotation speed of the model when character priority is high
        /// </summary>
        [Range(-1, 50)]
        [SerializeField, Indent] int _turnSpeed = 15;

        /// <summary>
        /// Brake strength.
        /// If this value is high, the character will stop quickly.
        /// </summary>
        [SerializeField, Indent] float _brakePower = 12;

        /// <summary>
        /// Acceleration.
        /// If this value is low, the character will accelerate slowly.
        /// </summary>
        [SerializeField, Indent] float _accelerator = 6;

        /// <summary>
        /// Movable slope.
        /// If the terrain is below this angle, move along the terrain.
        /// </summary>
        [SerializeField, Indent] float _angle = 45;

        /// <summary>
        /// Direction of movement.
        /// If anything other than Vector3.zero is specified,
        /// the character's direction of movement is limited to the direction of the specified axis.
        /// </summary>
        [SerializeField, Indent] Vector3 _lockAxis = Vector3.zero;


        [Title("Priorities")]

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

        private IGroundContact _groundCheck;
        private bool _hasGroundCheck;
        private Transform _transform;
        private Vector3 _moveDirection = Vector3.forward;
        private float _currentSpeed;
        private ActorSettings _actorSettings;
        private IBrain _brain;
        private Vector2 _inputValue;
        private bool _hasInput;
        private float _yawAngle;
        private bool _isTurning;


        /// ----------------------------------------------------------------------------
        // Properity

        /// <summary>
        /// Current movement speed. If priority is 0, the movement speed is also 0.
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
        /// ˆÚ“®•ûŒü‚Ì•Ï‰»—Ê (degree)
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

        /// <summary>
        /// 
        /// </summary>
        int IUpdateComponent.Order => Order.Control;


        // references

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
        // MeonoBehaviour Method

        private void Awake() {
            // 
            _actorSettings = gameObject.GetComponentInParent<ActorSettings>();
            
            // 
            _transform = _actorSettings.GetComponent<Transform>();
            _brain = _actorSettings.GetComponent<IBrain>();
            _hasGroundCheck = _actorSettings.TryGetComponent(out _groundCheck);
        }

        private void OnDestroy() {
            
        }

        void IUpdateComponent.OnUpdate(float deltaTime) {
            using var profiler = new ProfilerScope(nameof(MoveControl));

            if (_hasInput) {
                var preDirection = _moveDirection;
                var cameraYawRotation = Quaternion.AngleAxis(_actorSettings.CameraTransform.rotation.eulerAngles.y, Vector3.up);
                var direction = new Vector3(_inputValue.x, 0, _inputValue.y);


                // Determines direction of movement according to camera orientation
                _moveDirection = cameraYawRotation * direction.normalized;

                if (IsLockAxis) {
                    var dot = Vector3.Dot(_moveDirection, _lockAxis);
                    _moveDirection = _lockAxis * Mathf.Round(Mathf.Clamp(dot * 100, -1, 1));
                }

                _currentSpeed = Mathf.Lerp(_currentSpeed, _moveSpeed, _accelerator * deltaTime);
                DeltaTurnAngle = Vector3.SignedAngle(preDirection, _moveDirection, Vector3.up);

            } else {

                DeltaTurnAngle = 0;
                _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _brakePower * deltaTime);
                if (_currentSpeed < _moveStopThreshold)
                    _currentSpeed = 0;
            }

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
        // Public Method

        /// <summary>
        /// Moves according to the stick input.
        /// </summary>
        /// <param name="leftStick">Direction of movement.</param>
        public void Move(Vector2 leftStick) {
            _inputValue = leftStick;
            _hasInput = leftStick.sqrMagnitude > 0;
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR

        private void OnDrawGizmosSelected() {

            var offset = new Vector3(0, 0.1f, 0);
            var position = transform.position + offset;

            // show lines when use Lock Axis.
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
            Gizmos.color = Color.green;
            Gizmos.DrawRay(position, MoveVelocity);
        }

#endif
    }
}

