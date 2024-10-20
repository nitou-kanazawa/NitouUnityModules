using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nitou.Timeline {

    public class TimelineLoopController : MonoBehaviour , ILoopController{

        [SerializeField] private bool _trigger;


        public bool ExitLoopTrigger {
            get {
#if UNITY_EDITOR
                // ※エディター編集時はループしない
                if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == false)
                    return true;
#endif
                return _trigger;
            }
            set => _trigger = value;
        }
    }

}