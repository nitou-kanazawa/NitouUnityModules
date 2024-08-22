using UnityEngine;

namespace nitou.EventChannel {
    using nitou.EventChannel.Shared;

    /// <summary>
    /// <see cref="int"/>型のイベントチャンネル
    /// </summary>
    [CreateAssetMenu(
        fileName = "Event_Int",
        menuName = AssetMenu.Prefix.EventChannel + "Int Event"
    )]
    public class IntEventChannel : EventChannel<int> { }

}