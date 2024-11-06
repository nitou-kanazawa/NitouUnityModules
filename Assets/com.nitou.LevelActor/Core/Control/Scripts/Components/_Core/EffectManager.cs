using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nitou.LevelActors.Controller.Core {
    
    using Controller.Interfaces.Core;

    internal sealed class EffectManager : IInitializable<GameObject> {

        private readonly List<IEffect> _components = new(); // List of additional acceleration components

        public Vector3 Velocity { get; private set; }

        public bool IsInitialized { get; private set; } = false;


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// �����������D
        /// </summary>
        public void Initialize(GameObject obj) {
            obj.GetComponentsInChildren(_components);

            IsInitialized = true;
        }

        /// <summary>
        /// ���x���v�Z����D
        /// </summary>
        public void CalculateVelocity() {
            using var _ = new ProfilerScope("Velocity Calculation");

            SumVelocities(_components, out var velocity);
            Velocity = velocity;
        }
        
        /// <summary>
        /// ���x�����Z�b�g����D
        /// </summary>
        public void ResetVelocity() {
            foreach (var effect in _components)
                effect.ResetVelocity();
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private static void SumVelocities(in List<IEffect> velocities, out Vector3 sum) {
            sum = Vector3.zero;
            foreach (var velocity in velocities)
                sum += velocity.Velocity;
        }
    }
}