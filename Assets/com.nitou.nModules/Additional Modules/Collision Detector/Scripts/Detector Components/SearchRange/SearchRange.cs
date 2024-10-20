using System.Linq;
using UniRx;
using UnityEngine;
using nitou.BachProcessor;
using Sirenix.OdinInspector;

namespace nitou.Detecor {

    /// <summary>
    /// 
    /// </summary>
    public partial class SearchRange : DetectorBase {

        [Title("Area Shape")]
        [SerializeField, Indent] private float _radius = 2f;

        // 内部処理用
        private readonly ReactiveCollection<GameObject> _hitObjects = new();
        Transform _transform;


        /// <summary>
        /// 
        /// </summary>
        public int Count => _hitObjects.Count;

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyReactiveCollection<GameObject> HitObjects => _hitObjects;


        /// ----------------------------------------------------------------------------
        // LifeCycle Events

        private void OnEnable() {
            SearchRangeSystem.Register(this, Timing);
            InitializeBufferOfCollidedCollision();      
        }

        private void OnDisable() {
            SearchRangeSystem.Unregister(this, Timing);
            InitializeBufferOfCollidedCollision();      
        }

        private void OnDestroy() {
            _hitObjects?.Dispose();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void OnUpdate(in Collider[] hitColliders) {

            // Perform collision detection.
            var count = Physics.OverlapSphereNonAlloc(_transform.position, _radius, hitColliders, _hitLayer, QueryTriggerInteraction.Ignore);

            // 
            var hitObjectsInThisFram = hitColliders
                .Take(count)
                .WithoutNull()
                .Select(col => DetectionUtil.GetHitObject(col, _cacheTargetType));

            // 同期させる
            _hitObjects.SynchronizeWith(hitObjectsInThisFram);
        }


        /// ----------------------------------------------------------------------------
        // Private Methods

        /// <summary>
        /// リストを初期化する.
        /// </summary>
        protected void InitializeBufferOfCollidedCollision() {
            _hitObjects.Clear();
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        /// <summary>
        /// 非選択時のギズモ表示
        /// </summary>
        private void OnDrawGizmos() {

            var position = transform.position;
            var color = Count > 0 ? Colors.GreenYellow : Colors.White;

            // Range
            var offset = (0.1f * _radius);
            Gizmos_.DrawWireCylinder(position, _radius, offset * 2f, color);

            if (_hitObjects.IsNullOrEmpty()) return;

            // Target
            foreach (var obj in _hitObjects) {
                Gizmos_.DrawSphere(obj.transform.position, 0.1f, Colors.Gray);
            }

        }

        /// <summary>
        /// 選択時のギズモ表示
        /// </summary>
        private void OnDrawGizmosSelected() {
            var position = transform.position;
            var color = Count > 0 ? Colors.GreenYellow : Colors.White;

            Gizmos_.DrawSphere(position, _radius, color.WithAlpha(0.2f));
        }
#endif
    }
}
