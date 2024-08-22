using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace nitou.Sound {
    public partial class Sound {

        /// <summary>
        /// BGMマネージャー
        /// </summary>
        private class BgmManager : SoundManagerBase {

            /// ----------------------------------------------------------------------------
            // Field & Properity

            private SoundSource _soundSource = null;

            /// <summary>
            /// 再生中のBGM
            /// </summary>
            public AudioClip CurrentBGM { get; private set; }

            /// <summary>
            /// 次に再生するBGM
            /// </summary>
            public AudioClip NextBGM { get; private set; }

            /// <summary>
            /// 再生中かどうか
            /// </summary>
            public bool IsPlaying => _soundSource.IsPlaying;

            /// <summary>
            /// フェードアウト中かどうか
            /// </summary>
            public bool IsFadeOuting { get; private set; }

            // BGMがフェードするのにかかる時間
            public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
            public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
            private float _bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;


            /// ----------------------------------------------------------------------------
            // MonoBehaviour Method

            private void Awake() {
                Initialize();
            }

            private void Update() {
                if (!IsFadeOuting) return;

                //徐々にボリュームを下げていき、ボリュームが0になったらボリュームを戻し次の曲を流す
                _soundSource.Volume -= Time.deltaTime * _bgmFadeSpeedRate;
                if (_soundSource.Volume <= 0) {
                    Stop();
                    _soundSource.Volume = _volume;

                    if (NextBGM != null) {
                        Play(NextBGM, true);
                    }
                }
            }

            /// ----------------------------------------------------------------------------
            // Public Method

            /// <summary>
            /// BGMを再生する
            /// </summary>
            public void Play(AudioClip clip, bool isLoop = true) {
                // クリップが空の場合，
                if (clip == null) {
                    Stop();
                    return;
                }

                // 同じ曲が指定された場合は処理しない
                if (CurrentBGM == clip) {
                    return;
                }

                // 再生中でなければ，そのまま流す
                if (!IsPlaying) {
                    _soundSource.Play(clip, isLoop);
                    CurrentBGM = clip;
                }
                // 再生中ならフェードアウトさせてから流す
                else {
                    IsFadeOuting = true;
                    NextBGM = clip;
                }

            }

            /// <summary>
            /// BGMを停止する
            /// </summary>
            public void Stop() {
                CurrentBGM = null;
                IsFadeOuting = false;
                _soundSource.Stop();
            }

            /// <summary>
            /// BGMをポーズする
            /// </summary>
            public void Pause() {
                _soundSource.Source.Pause();
            }

            /// <summary>
            /// BGMのポーズを解除する
            /// </summary>
            public void UnPause() {
                _soundSource.Source.UnPause();
            }


            /// ----------------------------------------------------------------------------
            // Private Method

            /// <summary>
            /// 初期化処理
            /// </summary>
            internal override void Initialize() {
                if (IsInitialized) return;

                var audioSouece = gameObject.AddComponent<AudioSource>();
                audioSouece.spatialBlend = 0;                               // ※2Dサウンドを有効化
                audioSouece.volume = Volume;
                _soundSource = new SoundSource(audioSouece, SoundType.BGM);

                // フラグ更新
                IsInitialized = true;
            }

            /// <summary>
            /// オーディオソースの設定を更新する
            /// </summary>
            internal override void SetVolume(float value) {
                _volume = Mathf.Clamp01(value);
                _soundSource.Volume = _volume;
            }


            /// ----------------------------------------------------------------------------
            // Static Method

            public static BgmManager Create() {
                var manager = new GameObject("[BGM Manager]").AddComponent<BgmManager>();
                manager.Initialize();
                return manager;
            }
        }

    }
}