using UnityEngine;

namespace nitou.CameraModule {

    /// <summary>
    /// <see cref="CameraLocationController"/>の被写体となるオブジェクト
    /// </summary>
    [SelectionBase]
    [DisallowMultipleComponent]
    public class CameraTarget : MonoBehaviour, ICameraTarget {
        
        /// <summary>
        /// 中心座標
        /// </summary>
        public Vector3 Position {
            get => transform.position;
            set => transform.position = value;
        }


    }

}