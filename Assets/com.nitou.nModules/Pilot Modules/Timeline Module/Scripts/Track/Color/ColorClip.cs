using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
#if UNITY_EDITOR
using System.ComponentModel;
#endif

namespace nitou.Timeline {
    
    [System.Serializable]
#if UNITY_EDITOR
    [DisplayName("Color Gradation Clip")]
#endif
    public class ColorClip : PlayableAsset, ITimelineClipAsset {

        public Gradient gradient;

        /// <summary>
        /// デフォルトのクリップ長さ
        /// </summary>
        public override double duration => TimelineConfig.DEFAULT_CLIP_LENGTH;

        /// <summary>
        /// クリップの振る舞い
        /// </summary>
        public ClipCaps clipCaps {
            get =>
                ClipCaps.Blending |
                ClipCaps.Extrapolation |
                ClipCaps.ClipIn;
        }


        /// ----------------------------------------------------------------------------
        // Override Method

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
            
            var playable = ScriptPlayable<ColorClipBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();
            behaviour.Clip = this;
            
            return playable;
        }
    }
}
