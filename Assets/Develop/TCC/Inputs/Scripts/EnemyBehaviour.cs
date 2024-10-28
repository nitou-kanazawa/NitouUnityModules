using UnityEngine;

namespace nitou.LevelActors.Inputs {


    public class EnemyBehaviour : EnemyBehaviourBase {

        [SerializeField]
        private Transform _target;


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method 

        private void Update() {
            if (_target != null) {
                SetMovement(_target.position - transform.position);
            }

        }

    }
}
