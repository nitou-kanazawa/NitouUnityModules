using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    /// <summary>
    /// シーン移動で破棄しないオブジェクト
    /// </summary>
    [DefaultExecutionOrder(GameConfigs.ExecutionOrder.FARST)]
    [DisallowMultipleComponent]
    public sealed class AutoDestroy : MonoBehaviour{

        private void Awake() {
            Destroy(this.gameObject);
        }
    }
}
