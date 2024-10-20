using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nitou.Timeline {

    /// <summary>
    /// 
    /// </summary>
    public interface ILoopController {

        /// <summary>
        /// ループの終了トリガー
        /// </summary>
        public bool ExitLoopTrigger{ get; set; }

    }

}