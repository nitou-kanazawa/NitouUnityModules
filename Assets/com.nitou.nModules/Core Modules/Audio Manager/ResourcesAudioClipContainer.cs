using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nitou.Audio {

    /// <summary>
    /// Resourcesフォルダ内のAudioClipを管理するクラス
    /// </summary>
    public class ResourcesAudioClipContainer {

        private readonly Dictionary<string, AudioClip> _bgmDic;
        private readonly Dictionary<string, AudioClip> _seDic;

        private const string BGM_PATH = "Audio/BGM";
        private const string SE_PATH = "Audio/SE";

        public ResourcesAudioClipContainer() {

            // リソースフォルダから全SE&BGMのファイルを読み込みセット
            _bgmDic = Resources.LoadAll<AudioClip>(BGM_PATH).ToDictionary(clip => clip.name, clip => clip);
            _seDic = Resources.LoadAll<AudioClip>(SE_PATH).ToDictionary(clip => clip.name, clip => clip);
        }

        /// <summary>
        /// BGM用のクリップを取得する
        /// </summary>
        public AudioClip GetBGM(string bgmName) {
            if (_bgmDic.TryGetValue(bgmName, out var bgmClip)) {
                return bgmClip;
            }
            Debug.LogWarning($"BGM {bgmName} は存在しません");
            return null;
        }

        /// <summary>
        /// SE用のクリップを取得する
        /// </summary>
        public AudioClip GetSE(string seName) {
            if (_seDic.TryGetValue(seName, out var seClip)) {
                return seClip;
            }
            Debug.LogWarning($"SE {seName} は存在しません");
            return null;
        }
    }
}
