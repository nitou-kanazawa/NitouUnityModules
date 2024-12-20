using System.Collections.Generic;
using UnityEngine;

namespace nitou.LevelActors.Controller.Core {
    
    using Controller.Interfaces.Core;

    internal sealed class UpdateComponentManager : IInitializable<GameObject> {

        private readonly List<IUpdateComponent> _updates = new();     // List of components to be updated at runtime

        public bool IsInitialized { get; private set; } = false;


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize(GameObject obj) {
            obj.GetComponentsInChildren(_updates);
            _updates.Sort((a, b) => a.Order - b.Order);

            IsInitialized = true;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Process(float deltaTime) {
            using var _ = new ProfilerScope("Component Update");
            foreach (var update in _updates) {
                update.OnUpdate(deltaTime);
            }
        }
    }
}