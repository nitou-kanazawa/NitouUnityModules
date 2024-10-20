using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.Detecor {

    /// <summary>
    /// 干渉検出のタイミングと範囲に関するデータ
    /// </summary>
    [System.Serializable]
    public class DetectionBox {

        /// <summary>
        /// 干渉検出を行うタイミング (Fixed time)
        /// </summary>
        [MinMaxSlider(0, 1)] public Vector2 timeRange;

        /// <summary>
        /// 干渉検出の範囲．
        /// </summary>
        public Shapes.Box volume;


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionBox() {
            timeRange = new Vector2(0.2f, 0.8f);
            volume = new Shapes.Box(Vector3.zero, Quaternion.identity, Vector3.one);
        }

        /// <summary>
        /// 指定時間が検出タイミング内かどうか確認する
        /// </summary>
        public bool IsInTimeRange(float fixedTime) {
            return timeRange.x <= fixedTime && fixedTime <= timeRange.y;
        }
    }
}
