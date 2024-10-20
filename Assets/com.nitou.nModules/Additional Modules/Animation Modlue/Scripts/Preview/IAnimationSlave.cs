using UnityEngine;

namespace nitou.AnimationModule{

    /// <summary>
    /// AnimationClip再生に応じて駆動するインターフェース
    /// </summary>
    public interface IAnimationSlave {

        /// <summary>
        /// 現在の再生時間
        /// </summary>
        public float NormalizedTime { get; set; }
    }
}
