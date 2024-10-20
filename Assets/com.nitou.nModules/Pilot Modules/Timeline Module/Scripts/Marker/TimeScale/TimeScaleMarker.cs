using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// [参考]
//  テラシュール: Timelineからメソッドを呼ぶ新機能 MarkerとSignal、Signal Receiver https://tsubakit1.hateblo.jp/entry/2018/12/10/233146
//  github: Unity-Technologies/TimelineMarkerCustomization https://github.com/Unity-Technologies/TimelineMarkerCustomization/tree/master

namespace nitou.Timeline{

    /// <summary>
    /// タイムスケールを変更するMarker
    /// </summary>
    [System.Serializable, DisplayName("TimeScale Marker")]
    public class TimeScaleMarker : Marker, INotification{

        [SerializeField] float _timeScale = 1f;
        public float TimeScale => _timeScale;

        /// <summary>
        /// マーカーの識別ID
        /// </summary>
        public PropertyName id => new PropertyName("TimeScale");
    }
}
