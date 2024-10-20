using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace nitou.Timeline{

    public class TimeScaleMarkerReceiver : MonoBehaviour, INotificationReceiver {

        /// <summary>
        /// 通知を受けた時の処理
        /// </summary>
        public void OnNotify(Playable origin, INotification notification, object context) {
            var marker = notification as TimeScaleMarker;
            if (marker == null) return;

            ChangeTimeScale(marker.TimeScale);
        }

        /// <summary>
        /// タイムスケールの変更
        /// </summary>
        private void ChangeTimeScale(float timeScale) {
            Time.timeScale = timeScale;
        }
    }
}
