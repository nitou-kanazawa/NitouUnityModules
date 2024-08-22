using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nitou.Sound {
    public partial class Sound {

        /// <summary>
        /// ボイスマネージャー
        /// </summary>
        private class VoiceManager : SoundManagerBase {

            /// ----------------------------------------------------------------------------
            // Field & Properity

            private AudioSource[] _sourceArray = null;

            // チャンネル数
            const int VOICE_CHANNEL = 4;


            /// ----------------------------------------------------------------------------
            // MonoBehaviour Method

            private void Awake() {
                Initialize();
            }


            /// ----------------------------------------------------------------------------
            // Private Method


            /// ----------------------------------------------------------------------------
            // Public Method

            /// <summary>
            /// VOICEを再生する
            /// </summary>
            public void Play(AudioClip clip) {
                // クリップが空の場合，
                if (clip == null) { return; }

                // 再生
                if (TryGetSource(out var source)) {
                    source.PlayOneShot(clip);
                }
                // ※未使用のソースが無い場合，
                else {
                    Debug.LogWarning("There are no idle audio source.");
                }
            }

            /// <summary>
            /// 全てのSEを停止する
            /// </summary>
            public void StopAll() {
                foreach (var source in _sourceArray) {
                    source.Stop();
                }
            }




            /// <summary>
            /// 初期化処理
            /// </summary>
            internal override void Initialize() {
                if (IsInitialized) return;

                _sourceArray = new AudioSource[VOICE_CHANNEL];
                for (int i = 0; i < VOICE_CHANNEL; i++) {
                    var audioSouece = gameObject.AddComponent<AudioSource>();
                    audioSouece.spatialBlend = 1;
                    audioSouece.volume = Volume;
                    _sourceArray[i] = audioSouece;
                }

                // フラグ更新
                IsInitialized = true;
            }

            /// <summary>
            /// オーディオソースの設定を更新する
            /// </summary>
            internal override void SetVolume(float value) {
                _volume = Mathf.Clamp01(value);
                foreach (var source in _sourceArray) {
                    source.volume = _volume;
                }
            }


            /// ----------------------------------------------------------------------------
            // Private Method

            /// <summary>
            /// 非再生中のオーディオソースを取得する
            /// </summary>
            private bool TryGetSource(out AudioSource source) {
                source = _sourceArray.First(s => !s.isPlaying);
                return source != null;
            }

            /// <summary>
            /// 指定したオーディオソースを取得する
            /// </summary>
            private AudioSource GetSource(int channel) =>
                (0 <= channel && channel < VOICE_CHANNEL) ? _sourceArray[channel] : null;


            /// ----------------------------------------------------------------------------
            // Static Method

            public static VoiceManager Create() {
                var manager = new GameObject("[Voice Manager]").AddComponent<VoiceManager>();
                manager.Initialize();
                return manager;
            }
        }

    }
}