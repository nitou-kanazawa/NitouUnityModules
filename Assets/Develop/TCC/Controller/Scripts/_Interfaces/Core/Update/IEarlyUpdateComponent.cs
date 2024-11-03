
namespace nitou.LevelActors.Interfaces.Components {

    /// <summary>
    /// EarlyUpdateBrain�̃^�C�~���O�ŃR���|�[�l���g�����s���邽�߂̃C���^�[�t�F�[�X�D
    /// Brain��FixedUpdate�œ��삷��ꍇ��FixedUpdate�̑O�ɁAUpdate�œ��삷��ꍇ��Update�̑O�Ɏ��s�����D
    /// </summary>
    public interface IEarlyUpdateComponent {

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