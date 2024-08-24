using System;
using UniRx;

// [参考]
//  qiita: Unityの新しいInputSystemをReactivePropertyに変換し使用する https://qiita.com/Yuzu_Unity/items/1d0b39d17888fda552bd

namespace UnityEngine.InputSystem {

    /// <summary>
    /// InputActionの基本的な拡張メソッド集
    /// </summary>
    public static class InputActionExtensions {

        /// ----------------------------------------------------------------------------
        #region As Observable

        /// <summary>
        /// "performed"イベントをObservableに変換する拡張メソッド
        /// </summary>
        public static IObservable<InputAction.CallbackContext> PerformedAsObservable(this InputAction inputAction) {
            return Observable.FromEvent<InputAction.CallbackContext>(
                h => inputAction.performed += h,
                h => inputAction.performed -= h);
        }

        /// <summary>
        /// "canceled"イベントをObservableに変換する拡張メソッド
        /// </summary>
        public static IObservable<InputAction.CallbackContext> CanceledAsObservable(this InputAction inputAction) {
            return Observable.FromEvent<InputAction.CallbackContext>(
                h => inputAction.canceled += h,
                h => inputAction.canceled -= h);
        }

        /// <summary>
        /// "started"イベントをObservableに変換する拡張メソッド
        /// </summary>
        public static IObservable<InputAction.CallbackContext> StartedAsObservable(this InputAction inputAction) {
            return Observable.FromEvent<InputAction.CallbackContext>(
                h => inputAction.started += h,
                h => inputAction.started -= h);
        }
        #endregion


        // ------------

        /// <summary>
        /// ReactivePropertyに変換する拡張メソッド
        /// </summary>
        public static ReadOnlyReactiveProperty<bool> GetButtonProperty(this InputAction inputAction) {

            return Observable.FromEvent<InputAction.CallbackContext>(
                    h => inputAction.performed += h,
                    h => inputAction.performed -= h)
                .Select(x => x.ReadValueAsButton())
                .ToReadOnlyReactiveProperty(false);
        }

        /// <summary>
        /// ReactivePropertyに変換する拡張メソッド (※Axis入力だと0等が反応しない)
        /// </summary>
        public static ReadOnlyReactiveProperty<float> GetAxisProperty(this InputAction inputAction) {

            return Observable.FromEvent<InputAction.CallbackContext>(
                    h => inputAction.performed += h,
                    h => inputAction.performed -= h)
                .Select(x => x.ReadValue<float>())
                .ToReadOnlyReactiveProperty(0);
        }

        /// <summary>
        /// ReactivePropertyに変換する拡張メソッド (※主にマウスで使用)
        /// </summary>
        public static ReadOnlyReactiveProperty<float> GetDeltaAxisProperty(this InputAction inputAction) {

            // ※Delta入力はUpdate基準なので変換
            return Observable.EveryUpdate()
                .Select(_ => inputAction.ReadValue<float>())
                .ToReadOnlyReactiveProperty(0);
        }



    }
}