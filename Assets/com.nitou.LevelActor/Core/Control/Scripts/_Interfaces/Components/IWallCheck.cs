using UnityEngine;

namespace nitou.LevelActors.Controller.Interfaces.Components {

    /// <summary>
    /// Interface for accessing wall detection.
    /// </summary>
    public interface IWallCheck {
    
        /// <summary>
        /// �ǂƐڐG���Ă��邩�ǂ����D
        /// </summary>
        public bool IsContact { get; }

        /// <summary>
        /// �ǂ̖@���x�N�g���D
        /// </summary>
        public Vector3 Normal { get; }
    }
}