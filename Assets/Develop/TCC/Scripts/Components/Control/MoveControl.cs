using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelActors.Control{
    using nitou.LevelActors.Interfaces.Components;

    public sealed class MoveControl : MonoBehaviour, IUpdateComponent {

        [SerializeField, Indent] float _moveSpeed = 4;

        int IUpdateComponent.Order => Order.Control;


        // references



        /// <summary>
        /// The direction the character wants to move in world coordinates.
        /// This value multiplied by Speed is the actual amount of movement.
        /// </summary>
        public Vector3 Direction { get; private set; }



        /// ----------------------------------------------------------------------------
        // MeonoBehaviour Method

        private void Awake() {
            
        }

        private void OnDestroy() {
            
        }

        void IUpdateComponent.OnUpdate(float deltaTime) {
            using var profiler = new ProfilerScope(nameof(MoveControl));


        }




        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void Reset() {
        }

#endif
    }
}
