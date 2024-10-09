using UnityEngine;

namespace nitou.CameraModule{

    /// <summary>
    /// カメラ制御コンポーネントのインターフェース
    /// </summary>
    public interface ICameraController{

        /// <summary>
        /// 制御対象
        /// </summary>
        public Camera Camera { get; }
    }
}
