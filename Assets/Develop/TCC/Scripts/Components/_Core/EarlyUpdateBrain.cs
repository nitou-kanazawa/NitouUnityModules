using UnityEngine;

namespace nitou.LevelActors.Core{

    [DefaultExecutionOrder(Order.EarlyUpdateBrain)]
    [AddComponentMenu("")]
    public class EarlyUpdateBrain : EarlyUpdateBrainBase{
        private void Update() => OnUpdate();
    }
}
