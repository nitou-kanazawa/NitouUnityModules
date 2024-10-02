using System.Collections.Generic;
using UnityEngine;

namespace nitou.LevelActors.Core {
    using nitou.LevelActors.Interfaces.Core;

    internal class EffectManager {

        private readonly List<IEffect> _components = new(); // List of additional acceleration components

        public Vector3 Velocity { get; private set; }


        /// ----------------------------------------------------------------------------
        // Public Method

        public void Initialize(GameObject obj) {
            obj.GetComponents(_components);
        }

        public void CalculateVelocity() {
            using var _ = new ProfilerScope("Velocity Calculation");
            SumVelocities(_components, out var velocity);
            Velocity = velocity;
        }

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