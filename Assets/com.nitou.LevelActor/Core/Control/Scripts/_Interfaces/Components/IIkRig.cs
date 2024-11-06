using UnityEngine;

namespace nitou.LevelActors.Controller.Interfaces.Components{

    public interface IIkRig  {

        /// <summary>
        /// Weight of the rig.
        /// </summary>
        float Weight { get; }
        
        /// <summary>
        /// Initialization method.
        /// </summary>
        void Initialize(Animator animator);
        
        /// <summary>
        /// Calculate weights and other factors before calculating IK.
        /// Update is not used to ensure IK works even when the object is inactive.
        /// </summary>
        void OnPreProcess(float deltaTime);
        
        /// <summary>
        /// Called at the timing of OnAnimatorIk.
        /// </summary>
        void OnIkProcess(Vector3 offset);
        
        /// <summary>
        /// Determines if the rig is usable.
        /// </summary>
        bool IsValid { get; }
    }
}