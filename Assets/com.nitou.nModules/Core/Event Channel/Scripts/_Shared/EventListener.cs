using System.Collections.Generic;
using UnityEngine;

namespace nitou.EventChannel.Shared {


    /// <summary>
    /// イベントリスナーの基底クラス
    /// </summary>
    public abstract class EventListener<TChannel> : MonoBehaviour
        where TChannel : EventChannel {

        // 
        public TChannel Channel = null;
        public event System.Action OnEventRaised = delegate { };

        /// <summary>
        /// チャンネルがセットされているかどうか
        /// </summary>
        public bool HaveChannel => Channel != null;


        /// ----------------------------------------------------------------------------
        // Public Method

        private void OnEnable() {
            if (Channel == null) return;
            Channel.OnEventRaised += Respond;
        }

        private void OnDisable() {
            if (Channel == null) return;
            Channel.OnEventRaised -= Respond;
        }


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// イベント発火時のレスポンス
        /// </summary>
        public void Respond() => OnEventRaised.Invoke();
    }



    /// <summary>
    /// イベントリスナーの基底クラス
    /// </summary>
    public abstract class EventListener<Type, TChannel> : MonoBehaviour
        where TChannel : EventChannel<Type> {

        // 
        public TChannel Channel = null;
        public event System.Action<Type> OnEventRaised = delegate { };

        /// <summary>
        /// チャンネルがセットされているかどうか
        /// </summary>
        public bool HaveChannel => Channel != null;


        /// ----------------------------------------------------------------------------
        // Public Method

        private void OnEnable() {
            if (Channel == null) return;
            Channel.OnEventRaised += Respond;
        }

        private void OnDisable() {
            if (Channel == null) return;
            Channel.OnEventRaised -= Respond;
        }


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// イベント発火時のレスポンス
        /// </summary>
        public void Respond(Type value) => OnEventRaised.Invoke(value);     // ※nullチェックはChannel側で行う
    }

}
