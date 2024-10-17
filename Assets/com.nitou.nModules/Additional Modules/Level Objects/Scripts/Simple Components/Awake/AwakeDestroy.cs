using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    /// <summary>
    /// シーン移動で破棄しないオブジェクト
    /// </summary>
    internal sealed class AwakeDestroy : AwakeBehaviour {

        /// <summary>
        /// 開始処理
        /// </summary>
        protected override void OnAwake() {
            Destroy(this.gameObject);
        }
    }
}
