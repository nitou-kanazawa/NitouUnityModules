using UnityEngine;

namespace nitou.LevelActors.Interfaces.Components{

    public interface IPooledObject{

        /// <summary>
        /// Initialize the object.
        /// Called by GameObjectPool.
        /// </summary>
        void Initialize(IGameObjectPool owner, bool hasRigidbody);
        
        /// <summary>
        /// The corresponding GameObject.
        /// </summary>
        GameObject GameObject { get; }

        /// <summary>
        /// An instance ID to identify the object.
        /// </summary>
        int InstanceId { get; }

        /// <summary>
        /// Called when the object is retrieved.
        /// </summary>
        void OnGet();
        
        /// <summary>
        /// True if the object is used.
        /// </summary>
        bool IsUsed { get; }
        
        /// <summary>
        /// Called when the object is released.
        /// </summary>
        void OnRelease();

        /// <summary>
        /// Release the component.
        /// </summary>
        void Release();
    }
}