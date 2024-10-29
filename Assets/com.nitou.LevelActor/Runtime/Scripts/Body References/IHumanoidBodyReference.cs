using UnityEngine;

namespace nitou.LevelActors.Humanoid {

    /// <summary>
    /// Humanoidボディへの参照用インターフェース．
    /// </summary>
    public interface IHumanoidBodyReference{

        public Transform transform { get; }
    }
}
