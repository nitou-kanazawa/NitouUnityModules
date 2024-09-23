using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    /// <summary>
    /// シーン移動で破棄しないオブジェクト
    /// </summary>
    public sealed class AwakeDontDestroyObject : AwakeBehaviour {

        protected override void OnAwake(){
            DontDestroyOnLoad(gameObject);
        }
    }
}

