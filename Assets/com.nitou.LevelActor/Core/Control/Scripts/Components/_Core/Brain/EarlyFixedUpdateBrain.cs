using UnityEngine;

namespace nitou.LevelActors.Controller.Core{
    using Controller.Shared;

    [DefaultExecutionOrder(Order.EarlyUpdateBrain)]
    [AddComponentMenu(MenuList.MenuBrain + "Early Fiexd Update")]
    public class EarlyFixedUpdateBrain : EarlyUpdateBrainBase{
        private void FixedUpdate() => OnUpdate();
    }
}