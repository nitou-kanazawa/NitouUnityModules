using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using nitou.DesignPattern.Pooling;

namespace nitou.LevelActors.Core{
    using nitou.LevelActors.Interfaces.Components;
    using nitou.LevelActors.Shared;

    /// <summary>
    /// 
    /// </summary>
    [SelectionBase]
    [DisallowMultipleComponent]
    [AddComponentMenu(MenuList.MenuBrain + nameof(ActorSettings))]
    public sealed class ActorSettings : MonoBehaviour{

        [Title("Body Settings")]
        [SerializeField, Indent] float _mass = 1;
        [SerializeField, Indent] float _height = 1.4f;
        [SerializeField, Indent] float _radius = 0.5f;

        [Title("Hierarchy")]
        [SerializeField, Indent] Transform _checkParent;
        [SerializeField, Indent] Transform _controlParent;
        [SerializeField, Indent] Transform _effectParent;

        [Title("Environment Settings")]        
        [SerializeField, Indent] LayerMask _environmentLayer;


        // Camera
        private Camera _camera;
        private Transform _cameraTransform;

        // GameObject配下のコライダーリスト
        private readonly List<Collider> _hierarchyColliders = new();

        // 定数
        private const float MIN_HEIGHT = 0.1f;
        private const float MIN_RADIUS = 0.1f;
        private const float MIN_MASS = 0.001f;


        /// ----------------------------------------------------------------------------
        // Properity

        /// <summary>
        /// 半径
        /// </summary>
        public float Radius {
            get => _radius;
            set {
                var newValue = Mathf.Max(value, MIN_RADIUS);
                if (Mathf.Approximately(_radius, newValue))
                    return;

                _radius = newValue;
            }
        }

        /// <summary>
        /// 身長
        /// </summary>
        public float Height {
            get => _height;
            set {
                _height = Mathf.Max(value, MIN_HEIGHT);
            }
        }

        /// <summary>
        /// 体重
        /// </summary>
        public float Mass {
            get => _mass;
            set => _mass = value;
        }

        /// <summary>
        /// Layer for recognizing terrain colliders.
        /// </summary>
        public LayerMask EnvironmentLayer => _environmentLayer;

        
        
        /// <summary>
        ///     Returns true if a camera is set.
        /// </summary>
        public bool HasCamera => _camera != null;

        /// <summary>
        ///     Gets character's camera information.
        ///     Uses Camera.Main if no camera is set.
        /// </summary>
        public Camera CameraMain {
            get {
                // Return the cached camera if it's already registered.
                if (_camera != null)
                    return _camera;

                ApplyMainCameraTransform();
                return _camera;
            }

            set {
                // Update the camera and _cameraTransform.
                _camera = value;
                _cameraTransform = _camera != null ? _camera.transform : null;
            }
        }

        /// <summary>
        ///     MainCamera's Transform.
        /// </summary>
        public Transform CameraTransform {
            get {
                // Get the camera's Transform if already registered.
                if (_cameraTransform != null)
                    return _cameraTransform;

                ApplyMainCameraTransform();

                return _cameraTransform;
            }
        }


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        private void Awake() {
            // Get a list of colliders.
            GatherOwnColliders();

            // Update the camera's Transform.
            ApplyMainCameraTransform();
        }

        /// <summary>
        ///     Callback when the component's values change.
        /// </summary>
        private void OnValidate() {
            // Ensure values don't go below the minimum.
            _height = Mathf.Max(MIN_HEIGHT, _height);
            _radius = Mathf.Max(MIN_RADIUS, _radius);
            _mass = Mathf.Max(MIN_MASS, _mass);

            UpdateSettings();
        }


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// 対象コライダーがボディ配下のものか確認する
        /// </summary>
        public bool IsOwnCollider(Collider collider) {
            return _hierarchyColliders.Contains(collider);
        }

        /// <summary>
        /// Retrieves the closest RaycastHit excluding the character's own colliders.
        /// </summary>
        public bool ClosestHit(RaycastHit[] hits, int count, float maxDistance, out RaycastHit closestHit) {
            
            var min = maxDistance;
            closestHit = default;
            var isHit = false;

            for (var i = 0; i < count; i++) {
                var hit = hits[i];

                // Skip if the current Raycast's distance is greater than the current minimum,
                // or if it belongs to the character's collider list, or if it's null.
                if (hit.distance > min || IsOwnCollider(hit.collider) || hit.collider == null)
                    continue;

                // Update the closest Raycast.
                min = hit.distance;
                closestHit = hit;

                // Set to true if at least one closest Raycast is found.
                isHit = true;
            }

            return isHit;
        }

        /// <summary>
        /// Actorコンポーネントを設定したGameObjectから取得する
        /// </summary>
        public bool TryGetActorComponent<T>(ActorComponent type, out T component){

            // コンポーネント
            Transform holder = type switch {
                ActorComponent.Check => _checkParent,
                ActorComponent.Effect => _effectParent,
                ActorComponent.Control => _controlParent,
                _ => this.transform
            };
            if (holder == null) holder = this.transform;

            return holder.TryGetComponent(out component);
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        /// <summary>
        /// 自身のボディ配下コライダー情報を更新する
        /// </summary>
        private void GatherOwnColliders() {
            _hierarchyColliders.Clear();
            _hierarchyColliders.AddRange(GetComponentsInChildren<Collider>());
        }

        /// <summary>
        ///     Updates <see cref="Camera.main" /> settings for <see cref="_camera" /> and <see cref="_cameraTransform" />.
        /// </summary>
        private void ApplyMainCameraTransform() {
            // Get objects with the MainCamera tag.
            _camera = Camera.main;

            // Update the CameraTransform if a camera is acquired.
            if (_camera != null && _cameraTransform == null) {
                _cameraTransform = _camera.transform;
            }
        }

        /// <summary>
        ///     Updates components with <see cref="IActorSettingUpdateReceiver" />.
        /// </summary>
        private void UpdateSettings() {
            var controls = ListPool<IActorSettingUpdateReceiver>.New();
            {
                // 更新
                GetComponents(controls);
                controls.ForEach(c => c.OnUpdateSettings(this));
            }
            controls.Free();
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void Reset() {
            _environmentLayer = LayerMaskUtil.OnlyDefault();
        }
#endif
    }


    public enum ActorComponent {
        Brain,
        Check,
        Effect,
        Control,
        Others,
    }


}
