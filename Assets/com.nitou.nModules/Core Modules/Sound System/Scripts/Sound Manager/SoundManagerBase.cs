using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nitou.Sound {

    /// <summary>
    /// AudioSourceをラップしたマネージャークラス
    /// </summary>
    public abstract class SoundManagerBase : MonoBehaviour {

        protected float _volume = 0.5f;
        protected bool _isMuted = false;

        /// <summary>
        /// ボリューム
        /// </summary>
        public virtual float Volume {
            get => _volume;
            set => SetVolume(value);
        }

        /// <summary>
        /// ミュート状態かどうか
        /// </summary>
        public virtual bool IsMuted {
            get => _isMuted;
            set => _isMuted = value;
        }

        /// <summary>
        /// 初期化が完了しているかどうか
        /// </summary>
        public bool IsInitialized {get; protected set;}



        /// ----------------------------------------------------------------------------
        // Private Method

        /// <summary>
        /// 初期化処理
        /// </summary>
        internal abstract void Initialize();

        /// <summary>
        /// 音量を設定する
        /// </summary>
        internal virtual void SetVolume(float value) {
            _volume = Mathf.Clamp01(value);
        }
    }

}