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
    public class HumanoidBodyReference : MonoBehaviour {

        /// ----------------------------------------------------------------------------
        #region Field

        [TabGroup("Hand")]
        [SerializeField, Indent] Transform _leftHand;

        [TabGroup("Hand")]
        [SerializeField, Indent] Transform _rightHand;

        // ----- 

        [TabGroup("Foot")]
        [SerializeField, Indent] Transform _leftFoot;

        [TabGroup("Foot")]
        [SerializeField, Indent] Transform _rightFoot;

        #endregion


        /// ----------------------------------------------------------------------------
        // Properity

        public Transform LeftHand => _leftHand;
        public Transform RightHand => _rightHand;



        /// ----------------------------------------------------------------------------

#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            {
                if (_rightHand != null)
                    Gizmos.DrawWireSphere(_rightHand.position, 0.1f);

                if (_leftHand != null)
                    Gizmos.DrawWireSphere(_leftHand.position, 0.1f);
            }

            {
                if (_leftFoot != null)
                    Gizmos.DrawWireSphere(_leftFoot.position, 0.1f);

                if (_rightFoot != null)
                    Gizmos.DrawWireSphere(_rightFoot.position, 0.1f);
            }
        }
#endif
    }

}