using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    /// <summary>
    /// シーン移動で破棄しないオブジェクト
    /// </summary>
    [DefaultExecutionOrder(GameConfigs.ExecutionOrder.FARST)]
    [DisallowMultipleComponent]
    public sealed class AutoDontDestroyObject : MonoBehaviour{

        private void Awake(){
            DontDestroyOnLoad(gameObject);
        }
    }
}

