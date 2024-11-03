using UnityEngine;

namespace nitou.LevelActors.Interfaces.Core{

    /// <summary>
    /// �A�N�^�[�ɉ����x��^���邽�߂̃C���^�[�t�F�[�X�D
    /// </summary>
    public interface IEffect  {

        /// <summary>
        /// �ǉ���������x
        /// </summary>
        Vector3 Velocity { get; }

        /// <summary>
        /// �����x�����Z�b�g����D
        /// </summary>
        void ResetVelocity();
    }
}