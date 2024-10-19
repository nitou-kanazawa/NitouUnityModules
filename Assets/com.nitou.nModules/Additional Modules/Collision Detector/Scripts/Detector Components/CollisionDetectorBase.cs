using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Sirenix.OdinInspector;
using nitou.BachProcessor;

// [参考] 
//  qiita: いまさらColliderのisTriggerを結合して判定する方法を知ったのでメモ https://qiita.com/tsujihaneta/items/ec656c2092e06a881ca8

namespace nitou.Detecor {

    /// <summary>
    /// 交差検出用コンポーネントの基底クラス．
    /// 具体的な検出処理は派生クラス側に定義する．
    /// </summary>
    public abstract class CollisionDetectorBase : ComponentBase {

        /// <summary>
        /// 実行タイミング
        /// </summary>
        [DisableInPlayMode]
        [EnumToggleButtons, HideLabel]
        [SerializeField, Indent] protected UpdateTiming Timing = UpdateTiming.Update;

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

        // --- 

        /// <summary>
        /// 衝突コライダーのリスト.
        /// 要素インデックスは <see cref="_hitObjects" />と対応している.
        /// </summary>
        protected readonly List<Collider> _hitColliders = new();

        /// <summary>
        /// 衝突コライダーを保有するオブジェクトのリスト．
        /// ※保有オブジェクトの判定は<see cref="_cacheTargetType"/>に依存する．
        /// </summary>
        protected readonly List<GameObject> _hitObjects = new();

        /// <summary>
        /// 現在のフレームで衝突したコライダーのリスト．
        /// </summary>
        protected readonly List<Collider> _hitCollidersInThisFrame = new();

        /// <summary>
        /// List of GameObjects hit during the current frame.
        /// ※保有オブジェクトの判定は<see cref="_cacheTargetType"/>に依存する．
        /// </summary>
        protected readonly List<GameObject> _hitObjectsInThisFrame = new();


        // Event Streem
        private readonly Subject<List<GameObject>> _onHitObjectsSubject = new();


        /// ----------------------------------------------------------------------------
        // Property

        /// <summary>
        /// ヒットしているかどうか．
        /// </summary>
        public bool IsHit => _hitColliders.Count > 0;

        /// <summary>
        /// 現在のフレームでヒットしているかどうか．
        /// </summary>
        public bool IsHitInThisFrame => _hitCollidersInThisFrame.Count > 0;

        /// <summary>
        /// 範囲内の衝突オブジェクトのリスト．
        /// </summary>
        public IReadOnlyList<GameObject> HitObjects => _hitObjects;

        /// <summary>
        /// 現在のフレームでヒットしたオブジェクトのリスト．
        /// </summary>
        public IReadOnlyList<GameObject> HitObjectsInThisFrame => _hitObjectsInThisFrame;

        /// <summary>
        /// コリジョンが検出されたときのイベント通知
        /// </summary>
        public System.IObservable<List<GameObject>> OnHitObjects => _onHitObjectsSubject;


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        protected virtual void Reset() {
            // 既定値は"Default"レイヤー
            _hitLayer = LayerMaskUtil.OnlyDefault();
        }

        protected virtual void OnDestroy() {
            _onHitObjectsSubject.OnCompleted();
            _onHitObjectsSubject.Dispose();
        }


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// <see cref="_hitObjects" /> に含まれるオブジェクトに対応するColliderを取得する．
        /// </summary>
        public Collider GetColliderForGameObject(GameObject obj) {
            var index = _hitObjects.IndexOf(obj);
            return index == -1 ? null : _hitCollidersInThisFrame[index];
        }


        /// ----------------------------------------------------------------------------
        // Protected Method

        /// <summary>
        /// リストを初期化する.
        /// </summary>
        protected void InitializeBufferOfCollidedCollision() {
            _hitColliders.Clear();
            _hitObjects.Clear();
            _hitObjectsInThisFrame.Clear();
            _hitCollidersInThisFrame.Clear();
        }

        /// <summary>
        /// イベントを発火する
        /// </summary>
        protected void RaiseOnHitEvent(List<GameObject> objects) {
            _onHitObjectsSubject.OnNext(objects);
        }
    }

}
