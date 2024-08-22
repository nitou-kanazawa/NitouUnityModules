using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    public class SimpleRotaion : MonoBehaviour, ISimpleMoveComponent {

        [SerializeField] float _angularSpeed = 30;

        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        private void Awake() {

        }

        private void Update() {
            transform.Rotate(transform.up, Time.deltaTime * _angularSpeed);
        }

        /// ----------------------------------------------------------------------------
        // Public Method

        public void ResetPosition() {
        }
    }
}
