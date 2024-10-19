using UnityEngine;

namespace nitou.LevelActors.Sensor{

    /// <summary>
    /// è“®‚Å
    /// </summary>
    public interface IManualSensor : ISensor {
               

        /// <summary>
        /// ƒXƒLƒƒƒ“‚ğÀs‚·‚é
        /// </summary>
        public void Scan();

        /// <summary>
        /// 
        /// </summary>
        public bool IsInSight(Collider collider);
    }
}
