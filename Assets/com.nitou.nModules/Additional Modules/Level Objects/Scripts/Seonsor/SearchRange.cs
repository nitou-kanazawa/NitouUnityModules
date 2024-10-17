using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;

namespace nitou.Subsystem {

    /// <summary>
    /// 範囲内の監視対象オブジェクトにアタッチするコンポーネント
    /// （※OnTriggerExitは範囲内で非アクティブになったオブジェクトに対して呼ばれないため，その簡易対策）
    /// </summary>
    internal class SignalSender : MonoBehaviour{
        // イベント
        public event System.Action<GameObject> OnDisableEvent = delegate { };
        
        // イベント発火
        private void OnDisable() => OnDisableEvent.Invoke(gameObject);
    }


    /// <summary>
    /// 範囲内の対象オブジェクトを検知するコンポーネント
    /// </summary>
    public abstract class SearchRange<T> : MonoBehaviour {

        #region Inner Class

        /// <summary>
        /// 対象を扱うためのクラス
        /// </summary>
        protected class TargetInfo {
            
            /// <summary>
            /// 対象型（※インターフェースも想定される）
            /// </summary>
            public T target;
            
            /// <summary>
            /// 対象型を持つゲームオブジェクト
            /// </summary>
            public GameObject gameObject;

            /// <summary>
            /// 監視用コンポーネント
            /// </summary>
            internal SignalSender sender;

            /// ----------------------------------------------------------------------------

            /// <summary>
            /// コンストラクタ
            /// </summary>
            internal TargetInfo(T target, GameObject obj, SignalSender sender) {
                this.target = target;
                this.gameObject = obj;
                this.sender = sender;
            }
        }
        #endregion


        [Title("探索範囲")]

        /// <summary>
        /// 探索範囲コライダー
        /// </summary>
        [ShowInInspector, ReadOnly]
        public SphereCollider Collider => _collidr ?? (_collidr = GetComponentInChildren<SphereCollider>());
        protected SphereCollider _collidr;

        /// <summary>
        /// 探索範囲の半径
        /// </summary>
        [ShowInInspector, ReadOnly]
        public float Radius {
            get => Collider.radius;
            set => Collider.radius = Mathf.Max(value, 0.01f);   // ※適当な下限値
        }

        [Title("対象オブジェクト")]

        /// <summary>
        /// 範囲内にターゲットが存在するかどうか
        /// </summary>
        [ShowInInspector, ReadOnly]
        public bool ExistTarget => _targetList.Count > 0;

        /// <summary>
        /// 範囲内のターゲット
        /// </summary>
        [ShowInInspector, ReadOnly]
        public IEnumerable<T> Targets => _targetList.Select(t=>t.target).ToList();

        /// <summary>
        /// リストの最初のターゲット
        /// </summary>
        public T FirstTarget => ExistTarget ? _targetList[0].target : default;



        // ターゲット情報を格納したリスト
        protected List<TargetInfo> _targetList = new();
        // 
        CompositeDisposable _disposables = new();


        /// ----------------------------------------------------------------------------
        // イベント

        /// <summary>
        /// ターゲットが更新されたときのイベント通知
        /// （※引数はリストにターゲットが存在するかどうか）
        /// </summary>
        public System.IObservable<bool> OnTargetListUpdated => _onTargetListUpdated;
        protected Subject<bool> _onTargetListUpdated = new();

        /// <summary>
        /// ターゲットが追加された時のイベント通知
        /// </summary>
        public event System.Action<T> OnAddTarget = delegate { };

        /// <summary>
        /// ターゲットが削除された時のイベント通知
        /// </summary>
        public event System.Action<T> OnRemoveTarget = delegate { };


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method 

        private void OnEnable() {
            // コライダーが無ければ，非アクティブ化する
            if (Collider == null) this.enabled = false;

            Collider.isTrigger = true;
            Collider.enabled = true;
        }

        private void OnDisable() {
            Collider.enabled = false;
        }

        private void OnDestroy() {
            // リストのクリア
            RemoveAllTarget();

            // 解放処理
            _onTargetListUpdated.Dispose();
            _disposables.Dispose();
        }

        private void OnTriggerEnter(Collider other) => AddTarget(other.gameObject);

        private void OnTriggerExit(Collider other) => RemoveTarget(other.gameObject);


        /// ----------------------------------------------------------------------------
        // Public Method 

        /// <summary>
        /// ターゲットをリストへ追加する
        /// </summary>
        protected void AddTarget(GameObject targetObj) {

            if (targetObj.TryGetComponent<T>(out var target)) {

                // 既にリスト登録されている場合，何もしない
                var info = _targetList.Find(t => t.gameObject == targetObj);
                if (info != null) { return; }

                // リストへ追加
                var sender = GetOrAttachSignalSender(targetObj);  // 監視用コンポーネント
                _targetList.Add(new TargetInfo(target, targetObj, sender));

                // 派生クラスの処理
                OnUpdateTarget();

                // イベント通知
                OnAddTarget.Invoke(target);
                _onTargetListUpdated.OnNext(ExistTarget);
            }
        }

        /// <summary>
        /// ターゲットをリストから除外する
        /// </summary>
        protected void RemoveTarget(GameObject targetObj) {

            if (targetObj.TryGetComponent<T>(out var target)) {

                // 既にリスト登録解除されている場合，何もしない
                var info = _targetList.Find(t => t.gameObject == targetObj);
                if (info == null) { return; }

                // リストから削除
                targetObj.RemoveComponent<SignalSender>();
                _targetList.Remove(info);

                // 派生クラスの処理
                OnUpdateTarget();

                // イベント通知
                OnRemoveTarget.Invoke(target);
                _onTargetListUpdated.OnNext(ExistTarget);
            }
        }

        /// <summary>
        /// ターゲットをリストから除外する
        /// </summary>
        public bool RemoveTarget(T target) {

            // リストに登録されていない場合，終了
            var info = FindInfo(target);
            if (info == null) {
                return false;
            }

            // リストから削除
            RemoveTarget(info.gameObject);

            return true;
        }

        /// <summary>
        /// 全てのターゲットをリストから除外する
        /// </summary>
        protected void RemoveAllTarget() {
            // 監視用オブジェクトの破棄
            _targetList.ForEach(t => t.sender.Destroy());

            // リストの解放
            _targetList.Clear();
            _onTargetListUpdated.OnNext(false);
        }

        /// <summary>
        /// T型のターゲットを指定して対応するリスト情報を取得する
        /// （※T型の==比較ができないので，派生クラスで実装する）
        /// </summary>
        protected abstract TargetInfo FindInfo(T target);


        /// ----------------------------------------------------------------------------
        // Private Method (監視用コンポーネント)

        /// <summary>
        /// 監視用コンポーネントをアタッチする
        /// </summary>
        private SignalSender GetOrAttachSignalSender(GameObject targetObj) {

            // アタッチ済みの場合は，そのまま返す
            if (targetObj.HasComponent<SignalSender>()) {
                return targetObj.GetComponent<SignalSender>();
            }
            // 非アタッチ状態なら，セットアップも行う
            else {
                var sender = targetObj.AddComponent<SignalSender>();
                sender.OnDisableEvent += RemoveTarget;
                return sender;
            }
        }


        /// ----------------------------------------------------------------------------
        // Protected Method 

        /// <summary>
        /// ターゲットが更新された時の処理
        /// </summary>
        protected virtual void OnUpdateTarget() { }

        /// <summary>
        /// 派生クラスでのイベント発火
        /// </summary>
        protected void RaiseAddTargetEvent(T target) => OnAddTarget.Invoke(target);

        /// <summary>
        /// 派生クラスでのイベント発火
        /// </summary>
        protected void RaiseRemoveTargetEvent(T target) => OnRemoveTarget.Invoke(target);


        /// ----------------------------------------------------------------------------
        // Debug

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (Collider == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Collider.transform.position, Collider.radius);
        }
#endif


        /// ----------------------------------------------------------------------------
        // Public Method 

        ///// <summary>
        ///// 最も近いターゲットを返す 
        ///// </summary>
        //public T GetNearestTarget() {
        //    if (!ExistTarget) return default(T);

        //    var pos = transform.position;
        //    return _targets
        //        .OrderBy(t => (t.Transform.position - pos).sqrMagnitude)
        //        .FirstOrDefault();
        //}

        ///// <summary>
        ///// 画面に映っている最も近いターゲットを返す 
        ///// </summary>
        //public IAimable GetNearestVisbleTarget() {
        //    if (!ExistTarget) return null;

        //    var pos = transform.position;
        //    return _targets
        //        .Where(t=>t.IsVisible)
        //        .OrderBy(t => (t.Transform.position - pos).sqrMagnitude)
        //        .FirstOrDefault();
        //}


    }

}