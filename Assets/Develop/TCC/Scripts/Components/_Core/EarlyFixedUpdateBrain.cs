using UnityEngine;

namespace nitou.LevelActors.Core{

    [DefaultExecutionOrder(Order.EarlyUpdateBrain)]
    [AddComponentMenu("")]
    public class EarlyFixedUpdateBrain : EarlyUpdateBrainBase{
        private void FixedUpdate() => OnUpdate();
    }
}