using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelActors.Inputs {

    /// <summary>
    /// 
    /// </summary>
    [DefaultExecutionOrder(int.MinValue)]
    [DisallowMultipleComponent]
    public sealed class PlayerBrain : ActorBrain {

        [TitleGroup("Settings")]
        [DisableInPlayMode]
        [SerializeField, Indent] InputHandler _inputHandler = null;


        /// ----------------------------------------------------------------------------
        // Public Method 

        /// <summary>
        /// 入力ハンドラーを設定する
        /// </summary>
        public void SetInputHandler(InputHandler inputHandler) {
            if (inputHandler == null) return;

            _inputHandler = inputHandler;
            _characterActions.Reset();
        }


        /// ----------------------------------------------------------------------------
        // Protected Method 

        /// <summary>
        /// 更新処理の実行
        /// </summary>
        protected override void UpdateBrainValues(float dt) {
            if (Time.timeScale == 0) return;

            // 値の更新
            _characterActions.SetValues(_inputHandler);
            _characterActions.Update(dt);
        }

      
        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnValidate() {
            if (_inputHandler == null) {
                _inputHandler = gameObject.GetOrAddComponent<InputSystemHandler>();
            }
        }
#endif
    }
}
