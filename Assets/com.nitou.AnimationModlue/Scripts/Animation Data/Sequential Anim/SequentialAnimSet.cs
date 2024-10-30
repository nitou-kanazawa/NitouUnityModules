using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;

namespace nitou.AnimationModule{

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class SequentialAnimSet : ISequentialAnimSet{

        [DarkBox]
        [SerializeField, Indent] AnimationClip _startClip;
        [DarkBox]
        [SerializeField, Indent] AnimationClip _loopClip;
        [DarkBox]
        [SerializeField, Indent] AnimationClip _endClip;


        /// <summary>
        /// 開始アニメーション
        /// </summary>
        public AnimationClip StartClip => _startClip;

        /// <summary>
        /// ループアニメーション
        /// </summary>
        public AnimationClip LoopClip => _loopClip;

        /// <summary>
        /// 終了アニメーション
        /// </summary>
        public AnimationClip EndClip => _endClip;
    }
}
