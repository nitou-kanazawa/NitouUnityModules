using UnityEngine;

namespace nitou.EventChannel {
    using nitou.EventChannel.Shared;

    /// <summary>
    /// <see cref="void"/>型のイベントチャンネル
    /// </summary>
    [CreateAssetMenu(
        fileName = "Event_Void",
        menuName = AssetMenu.Prefix.EventChannel + "Void Event"
    )]
    public class VoidEventChannel : EventChannel { }

}