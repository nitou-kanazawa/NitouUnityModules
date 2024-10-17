using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    /// <summary>
    /// シーン開始時に子オブジェクトをすべて削除するコンポーネント
    /// </summary>
    internal sealed class AwakeDestroyChildrenComponent : AwakeBehaviour{

        /// <summary>
        /// 開始処理
        /// </summary>
        protected override void OnAwake() {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }
    }
}
