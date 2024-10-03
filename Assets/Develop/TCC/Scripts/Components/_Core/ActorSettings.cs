using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelActors.Core{

    [DisallowMultipleComponent]
    public sealed class ActorSettings : MonoBehaviour{

        [Title("Environment Settings")]        
        [SerializeField, Indent] LayerMask _environmentLayer;

        [Title("Body Settings")]
        [SerializeField, Indent] float _mass = 1;
        [SerializeField, Indent] float _height = 1.4f;
        [SerializeField, Indent] float _radius = 0.5f;


        [SerializeField, Indent] private Camera _camera;

        /// <summary>
        ///     Cached value of the camera's Transform.
        /// </summary>
        private Transform _cameraTransform;

        // List to store Collider components under GameObject.
        private readonly List<Collider> _hierarchyColliders = new();

        // 定数
        private const float MIN_HEIGHT = 0.1f;
        private const float MIN_RADIUS = 0.1f;
        private const float MIN_MASS = 0.001f;





        /// ----------------------------------------------------------------------------
        // Properity

        /// <summary>
        /// Layer for recognizing terrain colliders.
        /// </summary>
        public LayerMask EnvironmentLayer => _environmentLayer;

        /// <summary>
        /// 半径
        /// </summary>
        public float Radius {
            get => _radius;
            set {
                _radius = Mathf.Max(value, MIN_RADIUS);
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
        ///     Returns true if a camera is set.
        /// </summary>
        public bool HasCamera => _camera != null;

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
            // Get a list of components.
            GatherOwnColliders();

            // Update the camera's Transform.
            ApplyMainCameraTransform();
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


        /// ----------------------------------------------------------------------------
        // Private Method

        /// <summary>
        /// 自身のコライダー情報を更新する
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
            if (_camera != null && _cameraTransform == null)
                _cameraTransform = _camera.transform;
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void Reset() {
            _environmentLayer = LayerMaskUtil.OnlyDefault();
        }

#endif

    }


}
