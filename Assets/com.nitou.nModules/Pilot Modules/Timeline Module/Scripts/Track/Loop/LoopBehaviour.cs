using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace nitou.Timeline {

    [System.Serializable]
    public class LoopBehaviour : PlayableBehaviour {

        public PlayableDirector director { get; set; }

        /// <summary>
        /// 終了フラグを管理するコンポーネント
        /// </summary>
        public ILoopController controller { get; set; }

        /// <summary>
        /// クリップ終了時の処理
        /// </summary>
        public override void OnBehaviourPause(Playable playable, FrameData info) {
            if (controller == null) return;

            // 終了トリガーが立っていれば，ループを抜ける
            if (controller.ExitLoopTrigger == true) {
                controller.ExitLoopTrigger = false;
                return;
            }

            // 再生時間をリセット
            director.time -= playable.GetDuration();
        }
    }

}