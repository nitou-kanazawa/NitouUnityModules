
namespace nitou.LevelActors.Controller.Interfaces.Core {

    /// <summary>
    /// Interface for updating the camera's orientation.
    /// Executed after the movement processing in <see cref="TinyCharacterController.Core.BrainBase"/> is completed.
    /// </summary>
    public interface ICameraUpdate {

        /// <summary>
        /// Updates the camera's orientation.
        /// </summary>
        void OnUpdate(float deltaTime);
    }
}