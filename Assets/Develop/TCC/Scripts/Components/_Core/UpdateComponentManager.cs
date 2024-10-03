using System.Collections.Generic;
using UnityEngine;

namespace nitou.LevelActors.Core {
    using nitou.LevelActors.Interfaces.Components;

    internal class UpdateComponentManager {

        private readonly List<IUpdateComponent> _updates = new();     // List of components to be updated at runtime

        public void Initialize(GameObject obj) {
            obj.GetComponents(_updates);
            _updates.Sort((a, b) => a.Order - b.Order);
        }

        public void Process(float deltaTime) {
            using var _ = new ProfilerScope("Component Update");
            foreach (var update in _updates) {
                update.OnUpdate(deltaTime);
            }
        }
    }
}