using UnityEngine;

namespace nitou.EventChannel {
    using nitou.EventChannel.Shared;

    /// <summary>
    /// <see cref="Color"/>型のイベントリスナー
    /// </summary>
    [AddComponentMenu(
        ComponentMenu.Prefix.EventChannel + "Color Event Listener"
    )]
    public class ColorEventListener : EventListener<Color, ColorEventChannel> { }

}