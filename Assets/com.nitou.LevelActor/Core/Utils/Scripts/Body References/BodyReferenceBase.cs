using UnityEngine;

namespace nitou.LevelActors.Humanoid {
    
    /// <summary>
    /// Humanoid�̃{�f�B�e�����Q�Ƃ��邽�߂̃R���|�[�l���g�D
    /// </summary>
    public abstract class BodyReferenceBase : MonoBehaviour, IHumanoidBodyReference {

        [SerializeField] Vector3 _offset;

    }
}
