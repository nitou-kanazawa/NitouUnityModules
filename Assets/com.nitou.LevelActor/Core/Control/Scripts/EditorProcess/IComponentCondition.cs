using System.Collections.Generic;

namespace nitou.LevelActors.Controller.Interfaces.Utils{

    public interface IComponentCondition{
        void OnConditionCheck(List<string> messageList);
    }
}