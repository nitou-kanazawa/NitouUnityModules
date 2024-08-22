using System;
using UniRx;

// [参考]
//  github: AnnulusGames/ReactiveInputSystem https://github.com/AnnulusGames/ReactiveInputSystem/blob/main/README_JA.md

namespace UnityEngine.InputSystem {

    /// <summary>
    /// PlayerActionの基本的な拡張メソッド集
    /// </summary>
    public static class PlayerInputExtensions {

        /// ----------------------------------------------------------------------------
        #region As Observable

        /// <summary>
        /// "onActionTriggerd"イベントをObservableに変換する拡張メソッド
        /// </summary>
        public static IObservable<InputAction.CallbackContext> OnActionTriggeredAsObservable(this PlayerInput playerInput) {
            return Observable.FromEvent<InputAction.CallbackContext>(
                h => playerInput.onActionTriggered += h,
                h => playerInput.onActionTriggered -= h);
        }

        /// <summary>
        /// "onControlsChanged"イベントをObservableに変換する拡張メソッド
        /// </summary>
        public static IObservable<PlayerInput> OnControlsChangedAsObservable(this PlayerInput playerInput) {
            return Observable.FromEvent<PlayerInput>(
                h => playerInput.onControlsChanged += h,
                h => playerInput.onControlsChanged -= h);
        }

        /// <summary>
        /// "onDeviceLost"イベントをObservableに変換する拡張メソッド
        /// </summary>
        public static IObservable<PlayerInput> OnDeviceLostAsObservable(this PlayerInput playerInput) {
            return Observable.FromEvent<PlayerInput>(
                h => playerInput.onDeviceLost += h,
                h => playerInput.onDeviceLost -= h);
        }

        /// <summary>
        /// "onDeviceRegained"イベントをObservableに変換する拡張メソッド
        /// </summary>
        public static IObservable<PlayerInput> OnDeviceRegainedAsObservable(this PlayerInput playerInput) {
            return Observable.FromEvent<PlayerInput>(
                h => playerInput.onDeviceRegained += h,
                h => playerInput.onDeviceRegained -= h);
        }
        #endregion

    }

}