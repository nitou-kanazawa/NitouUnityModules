using UnityEngine;

namespace nitou.LevelActors.Core{
    using nitou.LevelActors.Shared;

    [DefaultExecutionOrder(Order.EarlyUpdateBrain)]
    [AddComponentMenu(MenuList.MenuBrain + "Early Update")]
    public class EarlyUpdateBrain : EarlyUpdateBrainBase{
        private void Update() => OnUpdate();
    }
}
