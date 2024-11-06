using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace nitou.LevelActors.Sensor{

    /// <summary>
    /// 交差検出コンポーネントのインターフェース．
    /// <see cref="Rigidbody"/>や<see cref="CharacterController"/>に依存せずに交差判定を行える．
    /// </summary>
    public interface IOverlapDetector {

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<GameObject> Objects { get; }

    }


    public interface IOverlapDetector<T> {

        /// <summary>
        /// 対象オブジェクトのリスト．
        /// </summary>
        public IReadOnlyReactiveCollection<T> Targets { get; }
    }


    
    /// <summary>
    /// <see cref="IOverlapDetector"/>の基本的な拡張メソッド集．
    /// </summary>
    public static class OverlapDetectorExtensions {

    }
       
}
