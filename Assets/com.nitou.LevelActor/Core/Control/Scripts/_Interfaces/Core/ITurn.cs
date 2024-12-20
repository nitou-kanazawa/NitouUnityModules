
namespace nitou.LevelActors.Controller.Interfaces.Core {

    /// <summary>
    /// アクターの方向制御インターフェース．
    /// </summary>
    public interface ITurn : IPriority<ITurn> {

        /// <summary>
        /// 方向の回転速度．
        /// </summary>
        int TurnSpeed { get; }

        /// <summary>
        /// 最終的な方向．
        /// </summary>
        float YawAngle { get; }
    }
}