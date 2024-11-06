using UnityEngine;

namespace nitou.LevelActors.Controller.Interfaces.Components{

    /// <summary>
    /// Interface for accessing the results of gravity's behavior
    /// </summary>
    public interface IGravity {

        /// <summary>
        /// —Ž‰º‘¬“x [unit/sec]
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
        /// Indicates if the character left the ground within the same frame.
        /// </summary>
        bool IsLeaved { get;  }

        /// <summary>
        /// Indicates if the character landed on the ground within the same frame.
        /// </summary>
        bool IsLanded { get;  }
    }
}