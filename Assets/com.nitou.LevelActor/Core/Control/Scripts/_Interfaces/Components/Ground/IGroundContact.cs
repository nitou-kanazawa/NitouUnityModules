using UnityEngine;

namespace nitou.LevelActors.Controller.Interfaces.Components{

    /// <summary>
    /// �A�N�^�[���n�ʂɐڐG���Ă��邩�ǂ����𔻒f����R���|�[�l���g�ɃA�N�Z�X���邽�߂̃C���^�[�t�F�[�X.
    /// </summary>
    public interface IGroundContact {

        /// <summary>
        /// �ڒn��Ԃ��̔���. 
        /// �����n���̎��O����Ȃǂɗp�����܂��Ȕ���
        /// </summary>
        bool IsOnGround { get; }
        
        /// <summary>
        /// �ڒn��Ԃ��̌����Ȕ���.
        /// </summary>
        bool IsFirmlyOnGround { get; }
        
        /// <summary>
        /// �n�ʂ���̑��ΓI�ȋ���.
        /// </summary>
        float DistanceFromGround { get; }
        
        /// <summary>
        /// ���݂̒n�ʂ̖@���x�N�g��.
        /// </summary>
        Vector3 GroundSurfaceNormal { get; }
        
        /// <summary>
        /// �n�ʂƐڂ��Ă���_.
        /// </summary>
        Vector3 GroundContactPoint { get; }
    }
}