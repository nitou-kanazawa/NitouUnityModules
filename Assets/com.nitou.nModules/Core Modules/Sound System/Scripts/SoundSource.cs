using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nitou.Sound {

    /// <summary>
    /// AudioListenerのラップクラス
    /// </summary>
    public class SoundSource {

        public string ID { get; private set; }
        public SoundType Type { get; private set; }
        public AudioSource Source { get; private set; }

        /// <summary>
        /// クリップ
        /// </summary>
        public AudioClip Clip => (Source != null) ? Source.clip : null;

        /// <summary>
        /// 音量
        /// </summary>
        public float Volume {
            get => Source.volume;
            set => Source.volume = value;
        }

        /// <summary>
        /// 再生中かどうか
        /// </summary>
        public bool IsPlaying => (Source != null) ? Source.isPlaying : false;

        /// <summary>
        /// ループ中かどうか
        /// </summary>
        public bool IsLoop => (Source != null) ? Source.loop : false;

        /// <summary>
        /// キャスト
        /// </summary>
        /// <param name="sound"></param>
        public static implicit operator AudioSource(SoundSource sound) => sound.Source;


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SoundSource(AudioSource audioSource, SoundType soundType) {
            Source = audioSource;
            Type = soundType;
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        public void Play(AudioClip audioClip, bool loop = false, float spatialBlend = 0, float maxDistance = 256) {
            ID = System.Guid.NewGuid().ToString("N");
            Source.loop = loop;
            Source.clip = audioClip;
            Source.spatialBlend = spatialBlend;
            Source.maxDistance = maxDistance;
            Source.Play();
        }

        /// <summary>
        /// サウンドを停止します
        /// </summary>
        public void Stop() {
            ID = "";
            Source.Stop();
        }

    }

}