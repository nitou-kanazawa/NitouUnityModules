using UnityEngine;

namespace nitou.LevelActors.Humanoid {
    
    /// <summary>
    /// Humanoidのボディ各部を参照するためのコンポーネント．
    /// </summary>
    public abstract class BodyReferenceBase : MonoBehaviour, IHumanoidBodyReference {

        [SerializeField] Vector3 _offset;

    }
}
