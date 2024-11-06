using System;
using UnityEngine;

namespace nitou.LevelActors.Controller.Interfaces.Components{

    /// <summary>
    /// �n�ʃI�u�W�F�N�g�Ɋւ�����ɃA�N�Z�X���邽�߂̃C���^�[�t�F�[�X.
    /// </summary>
    public interface IGroundObject{

        /// <summary>
        /// �n�ʃI�u�W�F�N�g���ω������Ƃ��� true ��Ԃ�.
        /// </summary>
        bool IsChangeGroundObject { get; }
        
        /// <summary>
        /// ���݂̒n�ʃI�u�W�F�N�g.
        /// </summary>
        GameObject GroundObject { get; }
        
        /// <summary>
        /// ���݂̒n�ʃR���C�_�[.
        /// </summary>
        Collider GroundCollider { get; }

        /// <summary>
        /// �n�ʃI�u�W�F�N�g�̕ω���ʒm����X�g���[��.
        /// </summary>
        IObservable<GameObject> OnGrounObjectChanged { get; }
    }
}