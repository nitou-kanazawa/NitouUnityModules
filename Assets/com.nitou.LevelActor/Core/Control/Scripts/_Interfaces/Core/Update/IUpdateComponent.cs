
namespace nitou.LevelActors.Controller.Interfaces.Core{

    /// <summary>
    /// �R���|�[�l���g���X�V���邽�߂̃C���^�[�t�F�[�X�D
    /// ���̃R���|�[�l���g�����s����ɂ́A�����I�u�W�F�N�g���<see cref="BrainBase"/>���p�������I�u�W�F�N�g�����݂���K�v������D
    /// </summary>
    public interface IUpdateComponent {

        /// <summary>
        /// ���������D
        /// </summary>
        int Order { get; }
        
        /// <summary>
        /// �X�V�����D
        /// </summary>
        void OnUpdate(float deltaTime);
    }
}