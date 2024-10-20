using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace nitou.Timeline {

    [System.Serializable]
    public class StopPlayableAsset : PlayableAsset {

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
            var playable = ScriptPlayable<StopPlayableBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();
            behaviour.director = go.GetComponent<PlayableDirector>();
            return playable;
        }

    }

}