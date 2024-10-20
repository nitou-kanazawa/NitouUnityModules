using UnityEngine;

namespace nitou.Detecor{

    /// <summary>
    /// 
    /// </summary>
    public sealed partial class SequentialCollisionDetector : CollisionHitDetector{


        /// ----------------------------------------------------------------------------
        // LifeCycle Events

        private void OnEnable() {
            SequentialCollisionDetectorSystem.Register(this, Timing);
            InitializeBufferOfCollidedCollision();      
        }

        private void OnDisable() {
            SequentialCollisionDetectorSystem.Unregister(this, Timing);
        }

        // <summary>
        /// 各フレームでの検出開始時の初期化処理
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
            var count = Physics.OverlapSphereNonAlloc(transform.position, 0.1f, hitColliders, _hitLayer, QueryTriggerInteraction.Ignore);

            // Register objects that are not in contact from the list obtained with OverlapSphereNonAlloc(...).
            for (var hitIndex = 0; hitIndex < count; hitIndex++) {
                var hit = hitColliders[hitIndex];

                var hitObject = DetectionUtil.GetHitObject(hit, _cacheTargetType);

                // Skip if the GameObject was previously hit or if its tag is not in the _hitTags array.
                // However, if nothing is set in _hitTags, it won't be skipped.
                if (_hitObjects.Contains(hitObject) || hitObject.ContainTag(_hitTagArray) == false)
                    continue;

                // Register the collider.
                _hitColliders.Add(hit);
                _hitObjects.Add(hitObject);
                _hitCollidersInThisFrame.Add(hit);
                _hitObjectsInThisFrame.Add(hitObject);
            }

        }

        /// ----------------------------------------------------------------------------
        // Private Method

    }
}
