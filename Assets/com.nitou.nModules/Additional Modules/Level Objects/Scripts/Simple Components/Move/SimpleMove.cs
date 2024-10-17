using UnityEngine;

namespace nitou.LevelObjects.SimpleComponents {

    [DisallowMultipleComponent]
    public class SimpleMove : MonoBehaviour , ISimpleMoveComponent{

        [SerializeField] float length = 5;

        // ì‡ïîèàóùóp
        private Vector3 _startPosition;
        private SmoothDampVector3 _smooth;
        private Vector3 _point1;
        private Vector3 _point2;

        private Vector3 _currentTarget;

        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        private void Awake() {
            _startPosition = transform.position;
            SetWayPoints(_startPosition);

            _currentTarget = _point2;
            _smooth = new SmoothDampVector3(Vector3.zero);
        }

        void Update() {


        }


        /// ----------------------------------------------------------------------------
        // Public Method

        public void ResetPosition() {
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private void SetWayPoints(Vector3 pos) {
            _point1 = pos;
            _point2 = pos + Vector3.forward * length;
        }

    }

}
