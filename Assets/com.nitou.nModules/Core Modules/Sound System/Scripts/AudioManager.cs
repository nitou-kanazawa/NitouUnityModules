using UnityEngine;
using System.Collections.Generic;
using nitou.DesignPattern;

// [参考]
//  kanのメモ帳: ぼくがかんがえたさいきょうのAudioManager2 https://kan-kikuchi.hatenablog.com/entry/AudioManager2
//  Unityの森: 汎用サウンドマネージャー(Sound Manager)の作り方 https://3dunity.org/unity-introduction/making-sound-manager/
//  qiita: PlayerPrefs の使い方やメリット/デメリット https://qiita.com/riekure/items/3fd4526b13d8e89a7fc6
//  フタバゼミ: サウンド再生のPlayとPlayOneShotの違い https://futabazemi.net/unity/play_playoneshothttps://futabazemi.net/unity/play_playoneshot

namespace nitou.Audio {

    /// <summary>
    /// BGMとSEの管理をするグローバルマネージャ
    /// （※小さなプロジェクトまたはプロトタイプとしての使用を想定）
    /// </summary>
    public class AudioManager : SingletonMonoBehaviour<AudioManager> {

        // ボリューム保存用のkeyとデフォルト値
        private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
        private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
        private const float BGM_VOLUME_DEFULT = 0.2f;
        private const float SE_VOLUME_DEFULT = 0.3f;

        // リソース情報
        private const string BGM_PATH = "Audio/BGM";
        private const string SE_PATH = "Audio/SE";

        // BGMがフェードするのにかかる時間
        public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
        public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
        private float _bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

        // 次流すBGM名、SE名
        private string _nextBGMName;
        private string _nextSEName;

        // 現在のボリューム
        public float BGMVolume { get; private set; }
        public float SEVolume { get; private set; }

        // BGMをフェードアウト中か
        private bool _isFadeOut = false;

        // BGM用、SE用に分けてオーディオソースを持つ
        private AudioSource _bgmSource;
        private List<AudioSource> _seSourceList;
        private const int SE_SOURCE_NUM = 10;

        // 全AudioClipを保持
        private Dictionary<string, AudioClip> _bgmDic, _seDic;


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        override protected void Awake() {

            if (!base.CheckInstance()) {
                Destroy(this);
                return;
            }

            DontDestroyOnLoad(this.gameObject);

            // オーディオリスナーおよびオーディオソースをSE+1(BGMの分)作成
            //gameObject.AddComponent<AudioListener>();
            for (int i = 0; i < SE_SOURCE_NUM + 1; i++) {
                gameObject.AddComponent<AudioSource>();
            }

            // 作成したオーディオソースを取得して各変数に設定、ボリュームも設定
            AudioSource[] audioSourceArray = GetComponents<AudioSource>();
            _seSourceList = new List<AudioSource>();

            // 
            BGMVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
            SEVolume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFULT);

            for (int i = 0; i < audioSourceArray.Length; i++) {
                audioSourceArray[i].playOnAwake = false;

                if (i == 0) {   // ----- BGM用
                    audioSourceArray[i].loop = true;
                    _bgmSource = audioSourceArray[i];
                    _bgmSource.volume = BGMVolume;

                } else {        // ----- SE用
                    _seSourceList.Add(audioSourceArray[i]);
                    audioSourceArray[i].volume = SEVolume;
                }

            }

            // リソースフォルダから全SE&BGMのファイルを読み込みセット
            _bgmDic = new Dictionary<string, AudioClip>();
            _seDic = new Dictionary<string, AudioClip>();

            object[] bgmList = Resources.LoadAll(BGM_PATH);
            object[] seList = Resources.LoadAll(SE_PATH);

            foreach (AudioClip bgm in bgmList) _bgmDic[bgm.name] = bgm;
            foreach (AudioClip se in seList) _seDic[se.name] = se;
        }

        private void Update() {
            if (!_isFadeOut)  return;

            //徐々にボリュームを下げていき、ボリュームが0になったらボリュームを戻し次の曲を流す
            _bgmSource.volume -= Time.deltaTime * _bgmFadeSpeedRate;
            if (_bgmSource.volume <= 0) {
                _bgmSource.Stop();
                _bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
                _isFadeOut = false;

                if (!string.IsNullOrEmpty(_nextBGMName)) {
                    PlayBGM(_nextBGMName);
                }
            }

        }


        /// ----------------------------------------------------------------------------
        // Public Method (SE)

        /// <summary>
        /// 指定したファイル名のSEを流す。第二引数のdelayに指定した時間だけ再生までの間隔を空ける
        /// </summary>
        public void PlaySE(string seName, float delay = 0.0f) {
            if (!_seDic.ContainsKey(seName)) {
                Debug.Log(seName + "という名前のSEがありません");
                return;
            }

            _nextSEName = seName;
            Invoke("DelayPlaySE", delay);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DelayPlaySE() {
            foreach (AudioSource seSource in _seSourceList) {
                if (!seSource.isPlaying) {
                    seSource.PlayOneShot(_seDic[_nextSEName] as AudioClip);
                    return;
                }
            }
        }


        /// ----------------------------------------------------------------------------
        // Public Method (BGM)

        /// <summary>
        /// 指定したファイル名のBGMを流す。
        /// ※既に流れている場合は前の曲をフェードアウトさせてから。
        /// ※第二引数のfadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
        /// </summary>
        public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH) {
            if (!_bgmDic.ContainsKey(bgmName)) {
                Debug.Log(bgmName + "という名前のBGMがありません");
                return;
            }

            //現在BGMが流れていない時はそのまま流す
            if (!_bgmSource.isPlaying) {
                _nextBGMName = "";
                _bgmSource.clip = _bgmDic[bgmName] as AudioClip;
                _bgmSource.Play();
            }
            //違うBGMが流れている時は、流れているBGMをフェードアウトさせてから次を流す。同じBGMが流れている時はスルー
            else if (_bgmSource.clip.name != bgmName) {
                _nextBGMName = bgmName;
                FadeOutBGM(fadeSpeedRate);
            }
        }

        /// <summary>
        /// BGMをすぐに止める
        /// </summary>
        public void StopBGM() {
            _bgmSource.Stop();
        }

        /// <summary>
        /// 現在流れている曲をフェードアウトさせる
        /// fadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
        /// </summary>
        public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW) {
            _bgmFadeSpeedRate = fadeSpeedRate;
            _isFadeOut = true;
        }


        /// ----------------------------------------------------------------------------
        // Public Method (音量設定)

        /// <summary>
        /// BGMボリュームの変更・保存
        /// </summary>
        public void ChangeBGMVolume(float bgmVolume, bool exeSave = false) {
            BGMVolume = Mathf.Clamp01(bgmVolume);
            _bgmSource.volume = BGMVolume;

            if (exeSave) {
                PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
            }
        }

        /// <summary>
        /// SEボリュームの変更・保存
        /// </summary>
        public void ChangeSEVolume(float seVolume, bool exeSave = false) {
            SEVolume = Mathf.Clamp01(seVolume);

            foreach (AudioSource seSource in _seSourceList) {
                seSource.volume = SEVolume;
            }

            if (exeSave) {
                PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
            }
        }

    }

}