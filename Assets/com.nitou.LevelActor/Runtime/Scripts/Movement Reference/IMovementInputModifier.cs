using UnityEngine;

namespace nitou.LevelActors
{
    public interface IMovementInputModifier{

        /// <summary>
        /// C³Œã‚Ì“ü—ÍƒxƒNƒgƒ‹
        /// </summary>
        public Vector3 ModifieredInputVector { get; }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateInputData(Vector2 movementInput);

        /// <summary>
        /// 
        /// </summary>
        public void ResetInputData();
    }
}
