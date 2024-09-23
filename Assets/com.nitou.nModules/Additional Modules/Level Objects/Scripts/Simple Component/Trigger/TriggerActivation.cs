using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents{


    public sealed class TriggerActivation : MonoBehaviour{

        [SerializeField] GameObject _target;

        private void OnTriggerEnter(Collider other) {

            _target.SetActive(true);
        }


        private void OnTriggerExit(Collider other) {
            _target.SetActive(false);
        }
    }
}
