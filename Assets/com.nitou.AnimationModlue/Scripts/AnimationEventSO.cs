using UnityEngine;
using Sirenix.OdinInspector;

// [参考]
//  Hatena: UnityのAnimatorでアニメーション遷移するときの自分なりの解 https://yutakaseda3216.hatenablog.com/entry/2016/08/19/112114

namespace nitou.AnimationModule {

    /// <summary>
    /// AnimatorのステートでAnimationと紐づけて実行されるイベントデータ
    /// </summary>
    public abstract class AnimationEventSO : SerializedScriptableObject {

        /// ----------------------------------------------------------------------------
        #region Inspecter Group（※インスペクタ表示用）

        // グループ
        protected const string CLIP_INFO = "クリップ情報";
        protected const string EVENT_INFO = "アニメーション イベント";
        #endregion


        /// ----------------------------------------------------------------------------
        #region Field & Properity

        // 汎用

        /// <summary>
        /// 説明文
        /// </summary>
        [TextArea(2, 3)]
        [SerializeField] private string _description = "";

        /// --- 
        // クリップ情報

        /// <summary>
        /// 対象クリップ
        /// </summary>
        [TitleGroup(CLIP_INFO), Indent]
        [SerializeField] private AnimationClip Clip;

        /// <summary>
        /// クリップの長さ[sec]
        /// </summary>
        [TitleGroup(CLIP_INFO), Indent]
        [ShowInInspector, ReadOnly]
        public float Length => (Clip != null) ? Clip.length : 0;

        /// <summary>
        /// クリップの長さ[sec]
        /// </summary>
        [TitleGroup(CLIP_INFO), Indent]
        [ShowInInspector, ReadOnly]
        public bool IsLoop => (Clip != null) ? Clip.isLooping : false;

        /// --- 
        // 内部処理用

        /// <summary>
        /// 全てのイベントが完了したかどうか
        /// </summary>
        public bool IsCompleted { get; protected set; }

        /// <summary>
        /// 現在，待機中のイベント情報
        /// （※実際のイベントデータのリストは派生クラスで定義）
        /// </summary>
        protected int _currentIndex;

        /// <summary>
        /// イベント実行タイミング評価時の許容誤差（※指定値に正規化時間を用いているのは修正するべき）
        /// </summary>
        protected static readonly float BREADTH_TIME = 0.01f;

        #endregion


        /// ----------------------------------------------------------------------------
        // Internal Method

        /// <summary>
        /// 初期化処理
        /// </summary>
        internal virtual void Initialize() {
            SortData();

            _currentIndex = 0;
            IsCompleted = false;
        }

        /// <summary>
        /// データを実行タイミング順にソートする
        /// </summary>
        internal abstract void SortData();


        /// ----------------------------------------------------------------------------
        // Internal Method

        /// <summary>
        /// イベントを実行するタイミングか評価する
        /// </summary>
        internal abstract bool CheckNormalizeTime(float currentNormalizedTime);

        /// <summary>
        /// 待機中のイベントを実行する
        /// </summary>
        internal abstract void ExecuteCurrentEvent(Animator animator);
    }



}