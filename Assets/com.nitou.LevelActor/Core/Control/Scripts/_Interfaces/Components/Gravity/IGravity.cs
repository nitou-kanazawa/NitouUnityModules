using UnityEngine;

namespace nitou.LevelActors.Controller.Interfaces.Components{

    /// <summary>
    /// Interface for accessing the results of gravity's behavior
    /// </summary>
    public interface IGravity {

        /// <summary>
        /// �������x [unit/sec]
        /// </summary>
        float FallSpeed { get; }

        /// <summary>
        /// Override gravity acceleration
        /// </summary>
        void SetVelocity(Vector3 velocity);
        
        /// <summary>
        /// Multiplier for the gravity acting on the character
        /// </summary>
        float GravityScale { get; }

        /// <summary>
        /// ���݂̃t���[���ŃL�����N�^�[���n�ʂ��痣�ꂽ�ꍇ��True�D
        /// </summary>
        bool IsLeaved { get;  }

        /// <summary>
        /// ���݂̃t���[���ŃL�����N�^�[�����n�����ꍇ��True�D
        /// </summary>
        bool IsLanded { get;  }
    }
}