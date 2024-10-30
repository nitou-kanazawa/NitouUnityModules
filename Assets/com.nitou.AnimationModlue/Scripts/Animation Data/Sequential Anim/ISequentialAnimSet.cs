using UnityEngine;

namespace nitou.AnimationModule{

    /// <summary>
    /// (開始 / 待機 / 終了) モーションで構成されるシンプルなアニメーションセット．
    /// </summary>
    public interface ISequentialAnimSet : IAnimSet{
        
        public AnimationClip StartClip { get; }
        public AnimationClip LoopClip { get; }
        public AnimationClip EndClip { get; }
    }
}
