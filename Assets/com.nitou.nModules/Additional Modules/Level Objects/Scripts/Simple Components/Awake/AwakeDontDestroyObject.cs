using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    /// <summary>
    /// シーン移動で破棄しないオブジェクト
    /// </summary>
    internal sealed class AwakeDontDestroyObject : AwakeBehaviour {

        /// <summary>
        /// 開始処理
        /// </summary>
        protected override void OnAwake(){
            DontDestroyOnLoad(gameObject);
        }
    }
}

