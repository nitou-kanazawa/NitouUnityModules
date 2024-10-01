
namespace nitou.LevelActors.Interfaces.Components {

    /// <summary>
    /// Interface for updating components.
    /// To execute this component, an object inheriting from <see cref="BrainBase"/> must exist on the same object.
    /// </summary>
    public interface IUpdateComponent {
        
        /// <summary>
        /// Component update process.
        /// </summary>
        void OnUpdate(float deltaTime);

        /// <summary>
        /// オーダー
        /// </summary>
        int Order { get; }
    }
}