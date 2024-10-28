using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelActors.Inputs{

    /// <summary>
    /// 
    /// </summary>
    [DefaultExecutionOrder(int.MinValue)]
    [DisallowMultipleComponent]
    public class EnemyBrain : ActorBrain {

        [TitleGroup("Settings")]
        public EnemyBehaviourBase _behaviour;




        /// ----------------------------------------------------------------------------
        // Private Method 

        /// <summary>
        /// 更新処理の実行
        /// </summary>
        protected override void UpdateBrainValues(float dt) {
            if (Time.timeScale == 0) return;

            // 値の更新
            _characterActions.SetValues(_behaviour.CharacterActions);
            _characterActions.Update(dt);
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        protected void OnValidate() {
            if (_behaviour == null) {
                _behaviour = gameObject.GetComponent<EnemyBehaviourBase>();
            }
        }
#endif
    }

}
