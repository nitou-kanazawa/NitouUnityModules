using UnityEngine;

namespace nitou.LevelActors.Controller.Interfaces.Components {
    using nitou.LevelActors.Controller.Core;

    /// <summary>
    /// Callback called when CharacterSettings values change.
    /// Mainly used for changing CharacterController or Collider sizes.
    /// </summary>
    public interface IActorSettingUpdateReceiver {

        /// <summary>
        /// CharacterSettings values have changed
        /// </summary>
        void OnUpdateSettings(ActorSettings settings);
    }
}