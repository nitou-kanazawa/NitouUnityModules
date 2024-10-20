using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// [参考]
//  qiita: Timelineからデバイスの振動を制御する https://qiita.com/nkjzm/items/27b2a6c9ed0cc844a83c

namespace nitou.Timeline{

    /// <summary>
    /// 振動イベントを通知するMarker
    /// </summary>
    [System.Serializable, DisplayName("Vibrate Marker")]
    public class VibrateMarker : Marker, INotification {

        public float Duration = 0.5f;
        [Range(0f, 1f)] public float Power = 0.5f;
        [Range(0f, 1f)] public float Frequency = 0.5f;

        /// <summary>
        /// マーカーの識別ID
        /// </summary>
        public PropertyName id => new PropertyName("Vibration");
    }
}
