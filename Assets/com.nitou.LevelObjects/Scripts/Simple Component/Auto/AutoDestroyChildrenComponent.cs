using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    /// <summary>
    /// シーン開始時に子オブジェクトをすべて削除するコンポーネント
    /// </summary>
    [DefaultExecutionOrder(GameConfigs.ExecutionOrder.FARST)]
    [DisallowMultipleComponent]
    public sealed class AutoDestroyChildrenComponent : MonoBehaviour{

        private void Awake() {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }
    }
}
