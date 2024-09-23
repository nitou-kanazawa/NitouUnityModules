using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    /// <summary>
    /// シーン移動で破棄しないオブジェクト
    /// </summary>
    public sealed class AwakeDestroy : AwakeBehaviour {

        protected override void OnAwake() {
            Destroy(this.gameObject);
        }
    }
}
