using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace nitou.SceneSystem {
    using nitou.Sound;

    /// <summary>
    /// シーン上への参照起点となるオブジェクト
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneEntryPoint : MonoBehaviour, ISceneEntryPoint {

        [EnumToggleButtons, HideLabel]
        [SerializeField] private SceneType _sceneType = SceneType.MainLevel;

        // カメラ
        [Title("Main Level Settings")]
        [ShowIf("@_sceneType", SceneType.MainLevel)]
        [SerializeField, Indent] Camera _sceneCamera;

        // BGM
        [ShowIf("@_sceneType", SceneType.MainLevel)]
        [SerializeField, Indent] AudioClip _bgmClip;


        /// ----------------------------------------------------------------------------
        // Properity

        public Camera SceneCamera => _sceneCamera;
        public AudioClip SceneBGM => _bgmClip;


        /// ----------------------------------------------------------------------------
        // Protected Method

        protected virtual UniTask OnLoadInternal() => UniTask.CompletedTask;

        protected virtual UniTask OnUnloadInternal() => UniTask.CompletedTask;

        protected virtual UniTask OnActivateInternal() => UniTask.CompletedTask;

        protected virtual UniTask OnDeactivateInternal() => UniTask.CompletedTask;


        /// ----------------------------------------------------------------------------
        #region Interface Method

        /// <summary>
        /// シーンが読み込まれた時の処理
        /// </summary>
        async UniTask ISceneEntryPoint.OnSceneLoadAsync() {

            if (_sceneCamera != null && SceneManager.sceneCount > 1) {
                _sceneCamera.gameObject.SetActive(false);
            }

            // 個別処理
            //Debug_.Log("OnLoadInternal");
            await OnLoadInternal();
        }

        /// <summary>
        /// シーンが解放された時の処理
        /// </summary>
        async UniTask ISceneEntryPoint.OnSceneUnloadAsync() {
            await OnUnloadInternal();
        }

        /// <summary>
        /// アクティブなシーンに設定された時の処理
        /// </summary>
        async UniTask ISceneEntryPoint.OnSceneActivateAsync() {

            // 共通処理
            switch (_sceneType) {
                case SceneType.MainLevel:

                    // カメラの切り替え
                    if (_sceneCamera != null) {
                        CameraUtil.DeactivateAllCamera();
                        _sceneCamera.gameObject.SetActive(true);
                        _sceneCamera.enabled = true;
                    }

                    // BGM再生
                    if (_bgmClip != null) {
                        Sound.PlayBGM(_bgmClip);
                    }

                    break;

                case SceneType.SubLevel:
                    break;

                case SceneType.Other:
                    break;

                default:
                    break;
            }

            // 個別処理
            await OnActivateInternal();
        }

        /// <summary>
        /// アクティブなシーンから解除された時の処理
        /// </summary>
        async UniTask ISceneEntryPoint.OnSceneDeactivateAsync() {

            // 共通処理


            // 個別処理
            await OnDeactivateInternal();
        }

        #endregion
    }
}
