using UnityEngine;
using System.Collections.Generic;
using nitou.DesignPattern;

namespace nitou.Audio {

    public class SimpleAudioManager : SingletonMonoBehaviour<SimpleAudioManager> {

        private const float BGM_VOLUME_DEFULT = 0.2f;
        private const float SE_VOLUME_DEFULT = 0.3f;

        public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
        public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
        private float _bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

        public float BGMVolume { get; private set; }
        public float SEVolume { get; private set; }

        private bool _isFadeOut = false;
        private string _nextBGMName;
        private string _nextSEName;

        private AudioSource _bgmSource;
        private List<AudioSource> _seSourceList;
        private const int SE_SOURCE_NUM = 10;

        private ResourcesAudioClipContainer _audioClipContainer;

        protected override void Awake() {
            if (!base.CheckInstance()) {
                Destroy(this);
                return;
            }

            DontDestroyOnLoad(this.gameObject);

            _audioClipContainer = new ResourcesAudioClipContainer();
            InitializeAudioSources();

            BGMVolume = PlayerPrefs.GetFloat("BGM_VOLUME_KEY", BGM_VOLUME_DEFULT);
            SEVolume = PlayerPrefs.GetFloat("SE_VOLUME_KEY", SE_VOLUME_DEFULT);
        }

        private void InitializeAudioSources() {
            _seSourceList = new List<AudioSource>();

            for (int i = 0; i < SE_SOURCE_NUM + 1; i++) {
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;

                if (i == 0) {
                    audioSource.loop = true;
                    _bgmSource = audioSource;
                    _bgmSource.volume = BGMVolume;
                } else {
                    _seSourceList.Add(audioSource);
                    audioSource.volume = SEVolume;
                }
            }
        }

        private void Update() {
            if (!_isFadeOut) return;

            _bgmSource.volume -= Time.deltaTime * _bgmFadeSpeedRate;
            if (_bgmSource.volume <= 0) {
                _bgmSource.Stop();
                _bgmSource.volume = BGMVolume;
                _isFadeOut = false;

                if (!string.IsNullOrEmpty(_nextBGMName)) {
                    PlayBGM(_nextBGMName);
                }
            }
        }

        public void PlaySE(string seName, float delay = 0.0f) {
            var seClip = _audioClipContainer.GetSE(seName);
            if (seClip == null) return;

            _nextSEName = seName;
            Invoke(nameof(DelayPlaySE), delay);
        }

        private void DelayPlaySE() {
            foreach (var seSource in _seSourceList) {
                if (!seSource.isPlaying) {
                    seSource.PlayOneShot(_audioClipContainer.GetSE(_nextSEName));
                    return;
                }
            }
        }

        public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH) {
            var bgmClip = _audioClipContainer.GetBGM(bgmName);
            if (bgmClip == null) return;

            if (!_bgmSource.isPlaying) {
                _nextBGMName = "";
                _bgmSource.clip = bgmClip;
                _bgmSource.Play();
            } else if (_bgmSource.clip.name != bgmName) {
                _nextBGMName = bgmName;
                FadeOutBGM(fadeSpeedRate);
            }
        }

        public void StopBGM() {
            _bgmSource.Stop();
        }

        public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW) {
            _bgmFadeSpeedRate = fadeSpeedRate;
            _isFadeOut = true;
        }

        public void ChangeBGMVolume(float bgmVolume, bool exeSave = false) {
            BGMVolume = Mathf.Clamp01(bgmVolume);
            _bgmSource.volume = BGMVolume;

            if (exeSave) {
                PlayerPrefs.SetFloat("BGM_VOLUME_KEY", BGMVolume);
            }
        }

        public void ChangeSEVolume(float seVolume, bool exeSave = false) {
            SEVolume = Mathf.Clamp01(seVolume);

            foreach (var seSource in _seSourceList) {
                seSource.volume = SEVolume;
            }

            if (exeSave) {
                PlayerPrefs.SetFloat("SE_VOLUME_KEY", SEVolume);
            }
        }
    }
}
