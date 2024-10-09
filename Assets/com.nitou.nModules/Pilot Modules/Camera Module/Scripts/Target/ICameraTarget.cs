using UnityEngine;

namespace nitou.CameraModule {

    /// <summary>
    /// ƒJƒƒ‰‚Ì”íÊ‘Ì
    /// </summary>
    public interface ICameraTarget {

        Vector3 Position { get;}
    }

    /// <summary>
    /// ƒJƒƒ‰‚Ì”íÊ‘Ì
    /// </summary>
    public interface ICameraTargetBounds {

        /// <summary>
        /// 
        /// </summary>
        Bounds Bound { get; }
    }

}