
namespace nitou.LevelActors.Interfaces.Core {

    /// <summary>
    /// �A�N�^�[�̕�������C���^�[�t�F�[�X�D
    /// </summary>
    public interface ITurn : IPriority<ITurn> {

        /// <summary>
        /// �����̉�]���x�D
        /// </summary>
        int TurnSpeed { get; }

        /// <summary>
        /// �ŏI�I�ȕ����D
        /// </summary>
        float YawAngle { get; }
    }
}