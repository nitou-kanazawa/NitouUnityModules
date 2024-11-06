using UnityEngine;

namespace nitou.LevelActors.Inputs{

    public abstract class EnemyBehaviourBase : MonoBehaviour {

        protected ActorActions _actions;

        /// <summary>
        /// アクションマップ
        /// </summary>
        public ActorActions CharacterActions => _actions;

        /// <summary>
        /// 
        /// </summary>
        //public LevelActor CharacterActor { get; private set; }


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method 

        protected virtual void Awake() {
            //CharacterActor = gameObject.GetComponentInBranch<CharacterActor>();
        }

        protected virtual void Reset() {
            _actions.Reset();
        }


        /// ----------------------------------------------------------------------------
        // Public Method 

        /// <summary>
        /// 移動入力を設定する
        /// </summary>
        public void SetMovement(Vector3 direction) {
            //(var x, _, var z) = Vector3.ProjectOnPlane(direction, CharacterActor.Up).ClampMagnitude01();
            //_actions.movement.value = new Vector2(x, z);
        }

        /// <summary>
        /// 攻撃入力を設定する
        /// </summary>
        public void RaiseAttack() {
            _actions.attack1.value = true;
        }
    }
}
