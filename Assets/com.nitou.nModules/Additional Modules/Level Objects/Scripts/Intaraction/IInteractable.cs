using UnityEngine;

namespace nitou.LevelObjects{

    /// <summary>
    /// レベル内のインタラクト可能なオブジェクト．
    /// </summary>
    public interface IInteractable{

        /// <summary>
        /// 優先順位．
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 
        /// </summary>
        Vector3 Position { get; set; }

        void Intaract();
    }
}
