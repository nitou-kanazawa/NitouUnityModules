using UnityEngine;

namespace nitou.LevelActors.Interfaces.Core {

    /// <summary>
    /// アクターの移動制御インターフェース．
    /// </summary>
    public interface IMove : IPriority<IMove> {

        /// <summary>
        /// 移動速度
        /// </summary>
        Vector3 MoveVelocity { get; }
    }
}