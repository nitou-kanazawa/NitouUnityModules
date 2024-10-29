using UnityEngine;

namespace nitou.LevelActors.Core{
    using nitou.LevelActors.Shared;

    [DefaultExecutionOrder(Order.EarlyUpdateBrain)]
    [AddComponentMenu(MenuList.MenuBrain + "Early Fiexd Update")]
    public class EarlyFixedUpdateBrain : EarlyUpdateBrainBase{
        private void FixedUpdate() => OnUpdate();
    }
}