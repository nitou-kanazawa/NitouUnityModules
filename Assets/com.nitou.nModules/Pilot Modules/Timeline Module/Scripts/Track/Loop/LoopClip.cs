using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace nitou.Timeline {

    [System.Serializable]
    public class LoopClip : PlayableAsset, ITimelineClipAsset {
        
        public ClipCaps clipCaps => ClipCaps.None; 

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            
            var playable = ScriptPlayable<LoopBehaviour>.Create(graph);

            LoopBehaviour beheviour = playable.GetBehaviour();
            beheviour.director = owner.GetComponent<PlayableDirector>();
            beheviour.controller = owner.GetComponent<TimelineLoopController>();
            return playable;
        }
    }

}