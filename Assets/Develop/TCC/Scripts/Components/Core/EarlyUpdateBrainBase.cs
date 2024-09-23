using System.Collections.Generic;
using UnityEngine;

namespace nitou.LevelActors.Core {
    using nitou.LevelActors.Interfaces.Core;

    public abstract class EarlyUpdateBrainBase : MonoBehaviour {

        private readonly List<IEarlyUpdateComponent> _updates = new();

        private void Awake() {
            GatherComponents();
        }

        protected void GatherComponents() {
            GetComponentsInChildren(_updates);
            _updates.Sort((a, b) => a.Order - b.Order);
        }

        protected void OnUpdate() {
            // If executed at the timing of FixedUpdate, deltaTime returns the value of FixedUpdate.
            var deltaTime = Time.deltaTime;

            foreach (var update in _updates) {
                update.OnUpdate(deltaTime);
            }
        }
    }
}