using UnityEngine;

namespace nitou.EventChannel {
    using nitou.EventChannel.Shared;

    /// <summary>
    /// AudioClip型のイベントリスナー 
    /// </summary>
    [AddComponentMenu(
        ComponentMenu.Prefix.EventChannel + "AudioClip Event Listener"
    )]
    public class AudioClipEventListener : EventListener<AudioClip, AudioClipEventChannel> { }
}
