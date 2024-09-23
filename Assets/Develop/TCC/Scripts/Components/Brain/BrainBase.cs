using UnityEngine;

namespace nitou.LevelActors.Brain{
    using nitou.LevelActors.Interfaces.Components;
    using nitou.LevelActors.Interfaces.Core;

    public abstract class BrainBase : MonoBehaviour, ITransform {

        protected Quaternion Rotation;
        protected Vector3 Position;

        Vector3 ITransform.Position {
            get => Position;
            set {
                SetPositionDirectly(value);
                Position = value;
            }
        }

        Quaternion ITransform.Rotation {
            get => Rotation;
            set {
                SetRotationDirectly(value);
                Rotation = value;
            }
        }

        /// ----------------------------------------------------------------------------
        /// 



        /// <summary>
        /// Move the position of the character to the <see cref="newPosition"/>.
        /// Unlike <see cref="Warp(UnityEngine.Vector3)"/>, it is affected by other vectors.
        /// </summary>
        /// <param name="newPosition">new position</param>
        /// <returns>move success</returns>
        protected abstract void SetPositionDirectly(in Vector3 newPosition);

        /// <summary>
        /// Turn the character to the <see cref="newRotation"/>.
        /// Unlink <see cref="Warp(UnityEngine.Quaternion)"/>, it is affected by other turn.
        /// </summary>
        /// <param name="newRotation">new rotation.</param>
        protected abstract void SetRotationDirectly(in Quaternion newRotation);
    }
}
