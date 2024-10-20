using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace nitou.Timeline {

    // A behaviour that is attached to a playable
    public class StopPlayableBehaviour : PlayableBehaviour {

        public PlayableDirector director;

        // Called when the owning graph starts playing
        public override void OnGraphStart(Playable playable) { }

        // Called when the owning graph stops playing
        public override void OnGraphStop(Playable playable) {}

        // Called when the state of the playable is set to Play
        public override void OnBehaviourPlay(Playable playable, FrameData info) { }

        // Called when the state of the playable is set to Paused
        public override void OnBehaviourPause(Playable playable, FrameData info) {
            //何故か最初のフレームに呼ばれる（info.FrameIDは０ではない）
            if (playable.GetTime() != 0)
                director.Pause();
        }

        // Called each frame while the state is set to Play
        public override void PrepareFrame(Playable playable, FrameData info) {}
    }

}