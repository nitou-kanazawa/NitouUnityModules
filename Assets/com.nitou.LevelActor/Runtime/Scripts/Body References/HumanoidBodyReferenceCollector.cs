using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelObjects.Humanoid {

    public enum BodyType {
        RightHand,
        LeftHand,
    }

    /// <summary>
    /// Humanoidのボディ参照を保持するコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class HumanoidBodyReferenceCollector : MonoBehaviour {

        /// ----------------------------------------------------------------------------
        #region Field

        [TabGroup("Hand")]
        [SerializeField, Indent] HandReference _leftHand;
        [TabGroup("Hand")]
        [SerializeField, Indent] HandReference _rightHand;

        // ----- 

        [TabGroup("Foot")]
        [SerializeField, Indent] FootReference _leftFoot;
        [TabGroup("Foot")]
        [SerializeField, Indent] FootReference _rightFoot;

        #endregion


        /// ----------------------------------------------------------------------------
        // Properity

        public Transform LeftHand => _leftHand.transform;
        public Transform RightHand => _rightHand.transform;



        /// ----------------------------------------------------------------------------

#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            {
                if (_rightHand != null)
                    Gizmos.DrawWireSphere(_rightHand.transform.position, 0.1f);

                if (_leftHand != null)
                    Gizmos.DrawWireSphere(_leftHand.transform.position, 0.1f);
            }

            {
                if (_leftFoot != null)
                    Gizmos.DrawWireSphere(_leftFoot.transform.position, 0.1f);

                if (_rightFoot != null)
                    Gizmos.DrawWireSphere(_rightFoot.transform.position, 0.1f);
            }
        }
#endif
    }

}