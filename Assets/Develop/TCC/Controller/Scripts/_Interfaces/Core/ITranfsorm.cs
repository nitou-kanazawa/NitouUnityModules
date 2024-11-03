using UnityEngine;

namespace nitou.LevelActors.Interfaces.Core {

    /// <summary>
    /// Transform�ȊO�̍��W���������т��ăA�N�Z�X���邽�߂̃C���^�[�t�F�[�X�D
    /// Update�O�ɒl���L���b�V�����AGet����ł̓L���b�V�����ꂽ�l���g�p����D
    /// Set����ł͕ύX�������Ƀ^�[�Q�b�g�R���|�[�l���g�ɔ��f�����D
    /// </summary>
    public interface ITransform {

        /// <summary>
        /// �ʒu�D
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// �����D
        /// </summary>
        Quaternion Rotation { get; set; }
    }
}