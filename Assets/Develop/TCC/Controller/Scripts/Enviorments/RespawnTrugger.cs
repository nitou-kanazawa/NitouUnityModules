using UnityEngine;

namespace nitou.LevelActors {
    using nitou.LevelActors.Core;

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