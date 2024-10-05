using System.Collections.Generic;

namespace nitou.LevelActors.Interfaces.Utils{

    public interface IComponentCondition{
        void OnConditionCheck(List<string> messageList);
    }
}