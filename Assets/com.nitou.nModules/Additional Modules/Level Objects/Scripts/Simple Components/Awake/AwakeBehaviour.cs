using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    [DefaultExecutionOrder(GameConfigs.ExecutionOrder.FARST)]
    [DisallowMultipleComponent]
    internal abstract class AwakeBehaviour : MonoBehaviour {

        private void Awake() {
            OnAwake();
        }

        protected abstract void OnAwake();
    }
}
