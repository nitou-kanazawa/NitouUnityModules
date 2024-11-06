using UnityEngine;

namespace nitou.LevelActors.Controller.Demo {
    using nitou.LevelActors.Controller.Core;

    public sealed class RespawnTrugger : MonoBehaviour {

        // 
        [SerializeField] Transform _respawnPoint;

        private void OnTriggerEnter(Collider other) {

            if (other.TryGetComponent<BrainBase>(out var brain)) {

                var position = _respawnPoint.position;
                var rotation = _respawnPoint.rotation;

                brain.Warp(position, rotation);
            }

        }

    }

}