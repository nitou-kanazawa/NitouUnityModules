using UnityEngine;

namespace nitou.AnimationModule{

    /// <summary>
    /// A component which takes the root motion from an <see cref="Animator"/> and applies it to a
    /// <see cref="Transform"/>.
    /// </summary>
    [AddComponentMenu("Animancer/Redirect Root Motion To Transform")]
    public class RedirectRootMotionToTransform : RedirectRootMotion<Transform>{

        protected override void OnAnimatorMove(){
            if (!ApplyRootMotion) return;

            Target.position += Animator.deltaPosition;
            Target.rotation *= Animator.deltaRotation;
        }

    }
}
