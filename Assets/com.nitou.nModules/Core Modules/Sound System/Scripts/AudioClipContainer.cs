using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nitou.Sound{

    public class AudioClipContainer{

        // 全AudioClipを保持
        private Dictionary<string, AudioClip> _clipDicti;

        /// <summary>
        /// 含まれるクリップ数
        /// </summary>
        public int Count => _clipDicti.Count;


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AudioClipContainer(string path) {

            _clipDicti = new Dictionary<string, AudioClip>();
            foreach (AudioClip clip in Resources.LoadAll(path)) {
                _clipDicti[clip.name] = clip;
            }
        }

        /// <summary>
        /// クリップを取得する
        /// </summary>
        public AudioClip GetClip(string clipName) {
            if (!_clipDicti.ContainsKey(clipName)) {
                Debug.Log(clipName + "という名前のclipがありません");
                return null;
            }
            return _clipDicti[clipName];
        }
    }
}
