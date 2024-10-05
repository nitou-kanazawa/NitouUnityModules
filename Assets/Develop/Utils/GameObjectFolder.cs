using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace nitou.Hierarchy{

    /// <summary>
    /// Unfolds the hierarchical structure of GameObjects below the object where this component is attached.
    /// </summary>
    [AddComponentMenu("")]
    public sealed class GameObjectFolder : MonoBehaviour{

#if UNITY_EDITOR

        /// <summary>
        /// Display objects below the GameObjectFolder.
        /// </summary>
        [SerializeField] bool _isVisible = false;

        /// <summary>
        /// UI comment.
        /// </summary>
        [Multiline(15)]
        [SerializeField] string _comment;

        /// <summary>
        /// Color of the Hierarchy displayed in the Inspector.
        /// </summary>
        [ColorUsage(false)]
        [SerializeField] Color _menuColor = Color.white;

        public List<GameObject> ChildObjects = new ();
        
        private void Reset() {
            // Update the object's position when it is created.
            var trs = transform;
            trs.position = Vector3.zero;
            trs.hideFlags = HideFlags.HideInInspector;
        }

#endif
    }
}
