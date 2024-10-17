using Sirenix.OdinInspector;

namespace nitou.LevelActors{

    /// <summary>
    /// アクター移動処理の基準とする座標系
    /// </summary>
    public enum MovementReferenceMode {

        /// <summary>
        /// グローバル座標
        /// </summary>
        [LabelText(SdfIconType.PinMapFill)]
        World,

        /// <summary>
        /// 外部座標系
        /// </summary>
        [LabelText(SdfIconType.PinMap)]
        External,

        /// <summary>
        /// アクター座標系
        /// </summary>
        [LabelText(SdfIconType.PersonFill)]
        Actor,
    }
}
