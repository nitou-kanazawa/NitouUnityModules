using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nitou.Sound {
    public partial class Sound {

        /// <summary>
        /// SEマネージャー
        /// </summary>
        private class SeManager : SoundManagerBase {

            /// ----------------------------------------------------------------------------
            // Field & Properity

            private AudioSource[] _sourceArray = null;

            // チャンネル数
            const int SE_CHANNEL = 10;


            /// ----------------------------------------------------------------------------
            // MonoBehaviour Method

            private void Awake() {
                Initialize();
            }


            /// ----------------------------------------------------------------------------
            // Internal Method

            /// <summary>
            /// 初期化処理
            /// </summary>
            internal override void Initialize() {
                if (IsInitialized) return;

                _sourceArray = new AudioSource[SE_CHANNEL];
                for (int i = 0; i < SE_CHANNEL; i++) {
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
            // Public Method

            /// <summary>
            /// SEを再生する
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



            /// ----------------------------------------------------------------------------
            // Private Method

            /// <summary>
            /// 非再生中のオーディオソースを取得する
            /// </summary>
            private bool TryGetSource(out AudioSource source) {
                source = _sourceArray.FirstOrDefault(s => !s.isPlaying);
                return source != null;
            }

            /// <summary>
            /// 指定したオーディオソースを取得する
            /// </summary>
            private AudioSource GetSource(int channel) =>
                (0 <= channel && channel < SE_CHANNEL) ? _sourceArray[channel] : null;



            /// ----------------------------------------------------------------------------
            // Static Method

            public static SeManager Create() {
                var manager = new GameObject("[SE Manager]").AddComponent<SeManager>();
                manager.Initialize();
                return manager;
            }
        }

    }
}