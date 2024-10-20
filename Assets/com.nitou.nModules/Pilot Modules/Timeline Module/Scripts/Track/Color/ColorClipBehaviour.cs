using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace nitou.Timeline {

    public class ColorClipBehaviour : PlayableBehaviour {


        public ColorClip Clip { get; set; }
        public Color OutputColor { get; private set; }


        /// ----------------------------------------------------------------------------
        // グラフの開始・終了時

        public override void OnGraphStart(Playable playable) {

        }

        public override void OnGraphStop(Playable playable) {

        }


        /// ----------------------------------------------------------------------------
        // クリップの生成・破棄時

        public override void OnBehaviourPlay(Playable playable, FrameData info) {

        }

        public override void OnBehaviourPause(Playable playable, FrameData info) {

        }


        /// ----------------------------------------------------------------------------
        // クリップの実行時

        public override void PrepareFrame(Playable playable, FrameData info) { }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData) {

            var t = playable.GetTime();
            var d = playable.GetDuration();     // ※playable.GetDuration()だとClipCaps.loopで∞が返されるらしい
            var a = (float)(t / d);
            OutputColor = Clip.gradient.Evaluate(a);
        }
    }
}
