
namespace nitou.LevelActors.Controller.Interfaces.Core{

    /// <summary>
    /// �R���|�[�l���g�̗D��x�Ɋ֘A�������C�t�T�C�N���R�[���o�b�N���`����C���^�[�t�F�[�X�D
    /// </summary>
    public interface IPriorityLifecycle<T>{

        /// <summary>
        /// �ō��D��x��ێ����Ă���ԂɁA����I�ɌĂяo�����R�[���o�b�N�D
        /// </summary>
        void OnUpdateWithHighestPriority(float deltaTime);

        /// <summary>
        /// �ō��D��x���l�������ۂɌĂяo�����R�[���o�b�N�D
        /// </summary>
        void OnAcquireHighestPriority();

        /// <summary>
        /// �ō��D��x���������ۂɌĂяo�����R�[���o�b�N�D
        /// </summary>
        void OnLoseHighestPriority();
    }

}