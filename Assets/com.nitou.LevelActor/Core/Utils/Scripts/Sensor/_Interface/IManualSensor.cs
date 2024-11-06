using UnityEngine;

namespace nitou.LevelActors.Sensor{

    /// <summary>
    /// �蓮��
    /// </summary>
    public interface IManualSensor : IOverlapDetector {
               

        /// <summary>
        /// �X�L���������s����
        /// </summary>
        public void Scan();

        /// <summary>
        /// 
        /// </summary>
        public bool IsInSight(Collider collider);
    }
}
