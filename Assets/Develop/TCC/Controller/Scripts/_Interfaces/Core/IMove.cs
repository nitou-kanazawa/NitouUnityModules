using UnityEngine;

namespace nitou.LevelActors.Interfaces.Core {

    /// <summary>
    /// �A�N�^�[�̈ړ�����C���^�[�t�F�[�X�D
    /// </summary>
    public interface IMove : IPriority<IMove> {

        /// <summary>
        /// �ړ����x
        /// </summary>
        Vector3 MoveVelocity { get; }
    }
}