using UniRx;
using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelActors.Effect {
    using nitou.LevelActors.Core;
    using nitou.LevelActors.Interfaces.Core;
    using nitou.LevelActors.Interfaces.Components;
    using nitou.LevelActors.Shared;

    /// <summary>
    /// A component that sets a custom acceleration for a character.
    /// The acceleration is set externally and is not changed by the component.
    /// The acceleration set here is reflected in the character by the Brain.
    /// </summary>
    [AddComponentMenu(MenuList.MenuEffect + nameof(AdditionalVelocity))]
    [DisallowMultipleComponent]
    public sealed class AdditionalVelocity : MonoBehaviour,
        IEffect {

        [SerializeField] private Vector3 _velocity;

        /// <summary>
        /// ë¨ìxÅD
        /// </summary>
        public Vector3 Velocity {
            get => _velocity;
            set => _velocity = value;
        }

        /// <summary>
        /// Speed to move
        /// </summary>
        public float Speed => Velocity.magnitude;


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// Reset velocity.
        /// </summary>
        public void ResetVelocity() {
            Velocity = Vector3.zero;
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            
            var startPosition = transform.position;
            var endPosition = startPosition + Velocity;

            Gizmos_.DrawRay(startPosition, Velocity, Colors.Blue);
            Gizmos_.DrawSphere(endPosition, 0.1f, Colors.Blue);
        }
#endif
    }
}