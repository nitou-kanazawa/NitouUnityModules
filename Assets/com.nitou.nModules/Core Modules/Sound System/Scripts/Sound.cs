using System.Linq;
using UniRx;
using UnityEngine;

namespace nitou.Sound {

    /// <summary>
    /// 各種サウンド機能を呼び出すためのStaticメソッドクラス
    /// </summary>
    public sealed partial class Sound {

        // インスタンス
        public static Sound Instance =>_instance ?? (_instance = new Sound());
        private static Sound _instance = null;

        // Level Objects
        private BgmManager Bgm { get; set; }
        private SeManager Se { get; set; }
        private VoiceManager Voice { get; set; }


        /// ----------------------------------------------------------------------------
        // Private Method (セットアップ)

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Sound() {
            if (_instance != null) return;
            Initialize();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        private void Initialize() {

            var rootObj = new GameObject("[Audio Manager]");
            rootObj.DontDestroyOnLoad();

            // BGMマネージャー
            Bgm = BgmManager.Create();
            Bgm.transform.SetParent(rootObj.transform);

            // SEマネージャー
            Se = SeManager.Create();
            Se.transform.SetParent(rootObj.transform);

            // Voiceマネージャー
            Voice = VoiceManager.Create();
            Voice.transform.SetParent(rootObj.transform);
        }


        /// ----------------------------------------------------------------------------
        // Static Method (BGM再生)

        /// <summary>
        /// BGMを再生する
        /// </summary>
        public static void PlayBGM(AudioClip audioClip, bool isLoop = true) {
            Instance.Bgm.Play(audioClip, isLoop);
        }

        /// <summary>
        /// BGMを停止する
        /// </summary>
        public static void StopBGM() {
            Instance.Bgm.Stop();
        }

        /// <summary>
        /// BGMをポーズする
        /// </summary>
        public static void PauseBGM() {
            Instance.Bgm.Pause();
        }

        /// <summary>
        /// BGMのポーズを解除する
        /// </summary>
        public static void UnPauseBGM() {
            Instance.Bgm.UnPause();
        }


        /// ----------------------------------------------------------------------------
        // Static Method (SE再生)

        /// <summary>
        /// SEを再生する
        /// </summary>
        public static void PlaySE(AudioClip audioClip) {
            Instance.Se.Play(audioClip);
        }

        /// <summary>
        /// 全てのSEを停止する
        /// </summary>
        public static void StopSE() {
            Instance.Se.StopAll();
        }


        /// ----------------------------------------------------------------------------
        // Static Method (Voice再生)


        /// ----------------------------------------------------------------------------
        // Static Method (Volume)

        /// <summary>
        /// BGMのボリュームを設定する
        /// </summary>
        public static void SetBgmVolume(float value) {
            Instance.Bgm.SetVolume(value);
        }

        /// <summary>
        /// SEのボリュームを設定する
        /// </summary>
        public static void SetSeVolume(float value) {
            Instance.Se.SetVolume(value);
        }

        /// <summary>
        /// Voiceのボリュームを設定する
        /// </summary>
        public static void SetVoiceVolume(float value) {
            Instance.Voice.SetVolume(value);
        }
    }

}