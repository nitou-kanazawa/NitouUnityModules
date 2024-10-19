using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace nitou.LevelActors.Sensor{

    /// <summary>
    /// センサのインターフェース
    /// </summary>
    public interface ISensor {

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<GameObject> Objects { get; }

    }


    public interface ISensor<T> {

        /// <summary>
        /// 対象オブジェクトのリスト．
        /// </summary>
        public IReadOnlyReactiveCollection<T> Targets { get; }
    }


    
    /// <summary>
    /// Sensor関連の拡張メソッド集．
    /// </summary>
    public static class SensorExtensions {

    }
       
}
