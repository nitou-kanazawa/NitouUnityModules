using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace nitou.LevelActors.Sensor{

    /// <summary>
    /// �������o�R���|�[�l���g�̃C���^�[�t�F�[�X�D
    /// <see cref="Rigidbody"/>��<see cref="CharacterController"/>�Ɉˑ������Ɍ���������s����D
    /// </summary>
    public interface IOverlapDetector {

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<GameObject> Objects { get; }

    }


    public interface IOverlapDetector<T> {

        /// <summary>
        /// �ΏۃI�u�W�F�N�g�̃��X�g�D
        /// </summary>
        public IReadOnlyReactiveCollection<T> Targets { get; }
    }


    
    /// <summary>
    /// <see cref="IOverlapDetector"/>�̊�{�I�Ȋg�����\�b�h�W�D
    /// </summary>
    public static class OverlapDetectorExtensions {

    }
       
}
