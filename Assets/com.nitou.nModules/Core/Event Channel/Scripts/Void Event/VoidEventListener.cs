using UnityEngine;

namespace nitou.EventChannel {
    using nitou.EventChannel.Shared;

    /// <summary>
    /// <see cref="void"/>型のイベントリスナー
    /// </summary>
    [AddComponentMenu(
        ComponentMenu.Prefix.EventChannel + "Void Event Listener"
    )]
    public class VoidEventListener : EventListener<VoidEventChannel> { }

}