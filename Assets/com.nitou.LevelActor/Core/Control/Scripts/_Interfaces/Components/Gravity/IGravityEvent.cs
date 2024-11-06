using UnityEngine.Events;

namespace nitou.LevelActors.Controller.Interfaces.Components{

    /// <summary>
    /// Callback events for landing and leaving
    /// </summary>
    public interface IGravityEvent{

        /// <summary>
        /// ’…’n‚µ‚½‚Æ‚«‚Ìˆ—D
        /// Receives the fall speed.
        /// </summary>
        UnityEvent<float> OnLanding { get; }
        
        /// <summary>
        /// Event for leaving the ground
        /// </summary>
        UnityEvent OnLeave { get; }
    }
}