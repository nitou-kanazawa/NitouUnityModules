using UnityEngine;

namespace nitou.LevelActors.Controller.Interfaces.Components{

    /// <summary>
    /// ���݈ʒu����^�[�Q�b�g�ʒu�܂ł̐��`��Ԉړ��𐧌䂷�邽�߂̃C���^�[�t�F�[�X�D
    /// </summary>
    internal interface ILinearInterpolationMove{

        /// <summary>
        /// ���`��Ԉړ��̐i�s�x��ݒ肷��D
        /// </summary>
        void SetNormalizedTime(float moveAmount, float turnAmount);

        /// <summary>
        /// ���`��Ԉړ��̃g�����W�V�������J�n����D
        /// </summary>
        void Play(PropertyName id);

        /// <summary>
        /// ���`��Ԉړ��̃g�����W�V�������I������D
        /// </summary>
        void Stop(PropertyName id);

        /// <summary>
        /// Play�����s�����Ƀ^�[�Q�b�g�ʒu�ֈړ�����D
        /// </summary>
        void FitTargetWithoutPlay(PropertyName id);
    }
}