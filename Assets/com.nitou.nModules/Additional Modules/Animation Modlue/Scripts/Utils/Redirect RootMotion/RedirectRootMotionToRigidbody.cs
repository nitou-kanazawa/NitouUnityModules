using UnityEngine;

namespace nitou.AnimationModule {

    /// <summary>
    /// A component which takes the root motion from an <see cref="Animator"/> and applies it to a
    /// <see cref="Rigidbody"/>.
    /// </summary>
    [AddComponentMenu("Animancer/Redirect Root Motion To Rigidbody")]
    public class RedirectRootMotionToRigidbody : RedirectRootMotion<Rigidbody> {

        protected override void OnAnimatorMove() {
            if (!ApplyRootMotion) return;

            Target.MovePosition(Target.position + Animator.deltaPosition);
            Target.MoveRotation(Target.rotation * Animator.deltaRotation);
        }
    }

}
