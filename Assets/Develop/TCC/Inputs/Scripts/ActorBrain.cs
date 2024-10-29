using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelActors.Inputs{

    public enum UpdateMode {

        /// <summary>
        /// FixedUpdateで入力を使う場合のモード（デフォルト）
        /// </summary>
        FixedUpdate,

        /// <summary>
        /// 
        /// </summary>
        Update
    }


    /// <summary>
    /// 入力情報<see cref="CharacterActions"/>の更新処理と外部公開を担うコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class ActorBrain: MonoBehaviour{

        [TitleGroup("Settings")]
        [Tooltip("Indicates when actions should be consumed.\n\n" +
            "FixedUpdate (recommended): use this when the gameplay logic needs to run during FixedUpdate.\n\n" +
            "Update: use this when the gameplay logic needs to run every frame during Update.")]
        [SerializeField, Indent] UpdateMode _updateMode = UpdateMode.FixedUpdate;

        [TitleGroup("Actor Actions")]
        [HideLabel, ReadOnly]
        [SerializeField, Indent] protected LevelActorActions _characterActions = new();

        // 内部処理用
        private bool _firstUpdateFlag = false;


        /// <summary>
        /// 現在，設定されているアクション
        /// </summary>
        public LevelActorActions CharacterActions => _characterActions;


        /// ----------------------------------------------------------------------------
        // Lifecycle Events

        protected virtual void OnEnable() {
            _characterActions.InitializeActions();
            _characterActions.Reset();
        }

        protected virtual void OnDisable() {
            _characterActions.Reset();
        }

        protected virtual void Update() {
            float dt = Time.deltaTime;

            if (_updateMode == UpdateMode.FixedUpdate) {
                if (_firstUpdateFlag) {
                    _firstUpdateFlag = false;
                    _characterActions.Reset();
                }
            } else {
                _characterActions.Reset();
            }
            // 更新処理
            UpdateBrainValues(dt);
        }

        protected virtual void FixedUpdate() {
            _firstUpdateFlag = true;
            if (_updateMode == UpdateMode.FixedUpdate) {
                UpdateBrainValues(0f);
            }
        }


        /// ----------------------------------------------------------------------------
        // Protected Method 

        /// <summary>
        /// 更新処理の実行
        /// </summary>
        protected abstract void UpdateBrainValues(float dt);
    }
}
