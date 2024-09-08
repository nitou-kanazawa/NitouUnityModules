using UnityEngine;

namespace nitou.GameSystem {
    using nitou.EventChannel.Shared;

    /// <summary>
    /// InGame用のイベントチャンネル
    /// </summary>
    [CreateAssetMenu(
        fileName = "Event_Game",
        menuName = AssetMenu.Prefix.EventChannel + "Game Event",
        order = AssetMenu.Order.Early
    )]
    public class GameEventChannel : EventChannel<GameEventData> { }
}
