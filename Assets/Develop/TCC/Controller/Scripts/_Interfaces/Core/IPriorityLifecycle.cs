
namespace nitou.LevelActors.Interfaces.Core{

    /// <summary>
    ///  This interface defines the lifecycle callbacks for a component with respect to its priority status.
    /// </summary>
    public interface IPriorityLifecycle<T>{

        /// <summary>
        /// Callback called during regular updates while having the highest priority.
        /// </summary>
        void OnUpdateWithHighestPriority(float deltaTime);

        /// <summary>
        /// Callback called when the highest priority is acquired.
        /// </summary>
        void OnAcquireHighestPriority();

        /// <summary>
        /// Callback called when the highest priority is lost.
        /// </summary>
        void OnLoseHighestPriority();
    }

}