using UnityEngine;

namespace nitou.LevelObjects{

    public class Item : MonoBehaviour, IInteractable {

        [SerializeField] MeshRenderer _meshRenderer;

        public int Priority => 0;

        public Vector3 Position { 
            get => transform.position; 
            set => transform.position = value; 
        }

        public void Intaract() {
            Debug_.Log($"Intaract!");
        }

        private void OnTriggerEnter(Collider other) {
            Debug.Log("Enter");
            _meshRenderer.material.color = Color.red;
        }

        private void OnTriggerExit(Collider other) {
            _meshRenderer.material.color = Color.gray;
        }

        
    }
}
