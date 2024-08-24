using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace nitou.GameSystem {

    public class AppInputHandler : IAppInputHandler{

        private readonly InputActionAsset _uiInput;
        private readonly InputActionAsset _playerInput;


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AppInputHandler(InputActionAsset uiInPut, InputActionAsset  playerInput) {
            _uiInput = uiInPut;
            _playerInput = playerInput;
            
            DisableAll();
        }
        
        /// <summary>
        /// 終了処理
        /// </summary>
        public void Dispose() {
            DisableAll();
        }


        /// ----------------------------------------------------------------------------
        // Public Method (アクティブ設定)

        /// <summary>
        /// UI操作を有効にする
        /// </summary>
        public void EnableUI() {
            _uiInput.Enable();
        }

        /// <summary>
        /// プレイヤー操作を有効にする
        /// </summary>
        public void EnablePlayer() {
            _playerInput.Enable();
        }

        /// <summary>
        /// UI操作を無効にする
        /// </summary>
        public void DisableUI() {
            _uiInput.Disable();
        }

        /// <summary>
        /// プレイヤー操作を無効にする
        /// </summary>
        public void DisablePlayer() {
            _playerInput.Disable();
        }

        /// <summary>
        /// 入力操作を無効にする
        /// </summary>
        public void DisableAll() {
            DisableUI();
            DisablePlayer();
        }
    }
}
