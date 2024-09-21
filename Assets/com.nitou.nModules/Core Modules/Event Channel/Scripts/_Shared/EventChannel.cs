using UnityEngine;
using Sirenix.OdinInspector;

// [参考]
//  youtube: Devlog 2｜スクリプタブルオブジェクトを使ったゲームアーキテクチャ https://www.youtube.com/watch?v=WLDgtRNK2VE

namespace nitou.EventChannel.Shared {

    /// <summary>
    /// イベントチャンネル用のたたき台となるScriptable Object
    /// </summary>
    public abstract class EventChannel : ScriptableObject {

#if UNITY_EDITOR
#pragma warning disable 0414
        // 説明文
        [Multiline]
        [SerializeField] private string _description = default;
#pragma warning restore 0414
#endif
        public event System.Action OnEventRaised = delegate { };


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// イベントの発火
        /// </summary>
        [Button("Raise")]
        public void RaiseEvent() => OnEventRaised.Invoke();
    }


    /// <summary>
    /// イベントチャンネル用のたたき台となるScriptable Object
    /// </summary>
    public abstract class EventChannel<Type> : ScriptableObject {

#if UNITY_EDITOR
#pragma warning disable 0414
        // 説明文
        [Multiline]
        [SerializeField] private string _description = default;
#pragma warning restore 0414
#endif
        public event System.Action<Type> OnEventRaised = delegate { };


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// イベントの発火
        /// </summary>
        [Button("Raise")]
        public void RaiseEvent(Type value) {
            if (value == null) {
                Debug.LogWarning($"[{name}] event argument is null.");
                return;
            }
            OnEventRaised.Invoke(value);
        }
    }
}