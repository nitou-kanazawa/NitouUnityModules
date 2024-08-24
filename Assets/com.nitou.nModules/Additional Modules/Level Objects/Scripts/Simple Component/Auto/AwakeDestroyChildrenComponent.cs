using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    /// <summary>
    /// シーン開始時に子オブジェクトをすべて削除するコンポーネント
    /// </summary>
    public sealed class AwakeDestroyChildrenComponent : AwakeBehaviour{

        protected override void OnAwake() {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }
    }
}
