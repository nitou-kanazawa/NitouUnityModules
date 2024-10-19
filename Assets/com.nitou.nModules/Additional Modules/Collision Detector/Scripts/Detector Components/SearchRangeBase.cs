using System.Linq;
using UniRx;
using UnityEngine;
using nitou.BachProcessor;
using Sirenix.OdinInspector;

namespace nitou.Detecor{

    /// <summary>
    /// 
    /// </summary>
    public class SearchRangeBase : ComponentBase{

        [Title("Hit Settings")]

        // コライダー保有オブジェクトの判定
        [DisableInPlayMode]
        [SerializeField, Indent] protected CachingTarget _cacheTargetType = CachingTarget.Collider;

        // 対象レイヤー
        [DisableInPlayMode]
        [SerializeField, Indent] protected LayerMask _hitLayer;

        // タグ判定を有無
        [DisableInPlayMode]
        [SerializeField, Indent] protected bool _useHitTag = false;

        // 対象タグ
        [ShowIf("@_useHitTag")]
        [DisableInPlayMode]
        [SerializeField, Indent] protected string[] _hitTagArray;



        [ReadOnly, ShowInInspector]
        public int Count => _hitObjects.Count;

        // ----------

        [SerializeField] private float _radius = 5f;

        private readonly ReactiveCollection<GameObject> _hitObjects = new();

        private Collider[] _hitColliders = new Collider[30];


        private void Start() {
            _hitObjects.ObserveCountChanged()
                .Subscribe(_ => Debug_.ListLog(_hitObjects, Colors.GreenYellow));
        }

        private void Update() {
            OnUpdate(_hitColliders);

        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void OnUpdate(in Collider[] hitColliders) {

            // Perform collision detection.
            var count = Physics.OverlapSphereNonAlloc(transform.position, _radius,hitColliders, _hitLayer, QueryTriggerInteraction.Ignore);

            // 
            var hitObjectsInThisFram = hitColliders
                .Take(count)
                .WithoutNull()
                .Select(col => DetectionUtil.GetHitObject(col, _cacheTargetType));

            // 同期させる
            _hitObjects.SynchronizeWith(hitObjectsInThisFram);           

        }




        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnDrawGizmos() {

            Gizmos_.DrawWireSphere(transform.position, _radius, Colors.GreenYellow);

            if (_hitObjects.IsNullOrEmpty()) return;

            foreach (var obj in _hitObjects) {
                Gizmos_.DrawSphere(obj.transform.position, 0.1f, Colors.Green.WithAlpha(0.5f));
            }

        }
#endif
    }
}
