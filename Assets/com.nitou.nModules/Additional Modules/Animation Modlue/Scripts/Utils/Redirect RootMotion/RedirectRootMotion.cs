using UnityEngine;
using Sirenix.OdinInspector;

// [参考]
//  _: Redirect Root Motion https://kybernetik.com.au/animancer/docs/manual/other/redirect-root-motion/

namespace nitou.AnimationModule {

    /// <summary>
    /// <see cref="Animator"/>のルートモーションを別オブジェクトへ適用するためのコンポーネント．
    /// </summary>
    /// <remarks>
    /// This can be useful if the character's <see cref="Rigidbody"/> or <see cref="CharacterController"/> is on a
    /// parent of the <see cref="UnityEngine.Animator"/> to keep the model separate from the logical components.
    /// </remarks>
    [RequireComponent(typeof(Animator))]
    public abstract class RedirectRootMotion<T> : MonoBehaviour {

        [Title("Target")]

        [SerializeField, Indent]
        [Tooltip("The Animator which provides the root motion")]
        private Animator _animator;

        [SerializeField, Indent]
        [Tooltip("The object which the root motion will be applied to")]
        private T _target;


        /// <summary>
        /// The <see cref="UnityEngine.Animator"/> which provides the root motion.
        /// </summary>
        public ref Animator Animator => ref _animator;

        /// <summary>
        /// ルートモーションが適用されるオブジェクト．
        /// </summary>
        public ref T Target => ref _target;

        /// <summary>
        /// ルートモーションが有効かどうか．
        /// </summary>
        public bool ApplyRootMotion => 
            Target != null && Animator != null && Animator.applyRootMotion;


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        protected virtual void OnValidate() {
            gameObject.TryGetComponent(out _animator);

            if (_target == null) {
                _target = transform.parent.GetComponentInParent<T>();
            }
        }

        protected abstract void OnAnimatorMove();

    }
}
