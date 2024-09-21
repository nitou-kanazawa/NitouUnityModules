using UnityEngine;

namespace nitou.EventChannel {
    using nitou.EventChannel.Shared;

    /// <summary>
    /// <see cref="Color"/>型のイベントチャンネル
    /// </summary>
    [CreateAssetMenu(
        fileName = "Event_Color",
        menuName = AssetMenu.Prefix.EventChannel + "Color Event"
    )]
    public class ColorEventChannel : EventChannel<Color> { }

}