using UnityEngine;

namespace nitou.LevelActors.Controller.Core{
    using nitou.LevelActors.Controller.Shared;

    [DefaultExecutionOrder(Order.EarlyUpdateBrain)]
    [AddComponentMenu(MenuList.MenuBrain + "Early Update")]
    public class EarlyUpdateBrain : EarlyUpdateBrainBase{
        private void Update() => OnUpdate();
    }
}
