using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
#if UNITY_EDITOR
using System.ComponentModel;
#endif

// [参考]
//  凹みTips: Timelineのカスタムトラックおよびクリップを作成して見た目をキレイにしてみた https://tips.hecomi.com/entry/2022/03/28/235336

namespace nitou.Timeline {

    [TrackClipType(typeof(ColorClip))]
    [TrackBindingType(typeof(Renderer))]
#if UNITY_EDITOR
    [DisplayName("Color Gradation Track")]
#endif
    public class ColorTrack : TrackAsset {

        /// ----------------------------------------------------------------------------
        // Override Method

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
            
            return ScriptPlayable<ColorTrackMixer>.Create(graph,inputCount);
        }

    }
}
