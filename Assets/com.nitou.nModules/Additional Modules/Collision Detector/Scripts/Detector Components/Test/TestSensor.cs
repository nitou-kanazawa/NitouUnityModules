using UnityEngine;
using Sirenix.OdinInspector;
using nitou.BachProcessor;

namespace nitou.Detecor {

    public partial class TestSensor : CollisionDetectorBase {

        // 1度に検出できるコリジョンの最大数.
        private const int CAPACITY = 30;

        [MinValue(0.1f)]
        [SerializeField] float _radius = 1f;

        // 内部処理用
        Transform _transform;


        /// ----------------------------------------------------------------------------
        // LifeCycle Events

        private void Awake() {
            _transform = transform;
        }

        private void OnEnable() {
            TestSensorSystem.Register(this, Timing);
            InitializeBufferOfCollidedCollision();      // キャッシュのクリア
        }

        private void OnDisable() {
            TestSensorSystem.Unregister(this, Timing);
        }



        /// ----------------------------------------------------------------------------
        // Private Methods

        /// <summary>
        /// Initialize data at the start of processing.
        /// </summary>
        private void PrepareFrame() {
            _hitCollidersInThisFrame.Clear();
            _hitObjectsInThisFrame.Clear();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void OnUpdate(in Collider[] hitColliders) {

            // Perform collision detection.
            var count = Physics.OverlapSphereNonAlloc(_transform.position, _radius, hitColliders, _hitLayer, QueryTriggerInteraction.Ignore);

            // Register objects that are not in contact from the list obtained with OverlapSphereNonAlloc(...).
            for (var hitIndex = 0; hitIndex < count; hitIndex++) {
                var hit = hitColliders[hitIndex];

                var hitObject = DetectionUtil.GetHitObject(hit, _cacheTargetType);

                // Skip if the GameObject was previously hit or if its tag is not in the _hitTags array.
                // However, if nothing is set in _hitTags, it won't be skipped.
                if (_hitObjects.Contains(hitObject) || hitObject.ContainTag(_hitTagArray) == false)
                    continue;

                // Register the collider.
                Debug.Log("Add");
                _hitColliders.Add(hit);
                _hitObjects.Add(hitObject);
                _hitCollidersInThisFrame.Add(hit);
                _hitObjectsInThisFrame.Add(hitObject);
            }


        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnDrawGizmos() {

            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(Collider.transform.position, Collider.radius);

            Gizmos_.DrawWireSphere(transform.position, _radius, Colors.GreenYellow);

            if (_hitColliders == null) return;
            foreach(var obj in _hitObjects) {
                Gizmos_.DrawSphere(obj.transform.position, 0.1f, Colors.Green.WithAlpha(0.5f));
            }

        }
#endif
    }
}
