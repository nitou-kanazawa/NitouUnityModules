using System.Collections.Generic;
using UnityEngine;

namespace nitou.LevelActors.Core {
    using nitou.LevelActors.Interfaces.Core;

    internal class CameraManager {

        private readonly List<ICameraUpdate> _cameraUpdates = new();  // List of Camera Control Components


        public void Initialize(GameObject obj) {
            obj.GetComponents(_cameraUpdates);
        }

        public void Process(float deltaTime) {
            using var _ = new ProfilerScope("Camera Update");

            // No limitation by priority.
            // The final orientation is determined by Cinemachine.
            foreach (var cameraUpdate in _cameraUpdates) {
                cameraUpdate.OnUpdate(deltaTime);
            }
        }
    }
}