using UnityEngine;
using VContainer.Unity;
using Sirenix.OdinInspector;

namespace nitou.SceneSystem {

    /// <summary>
    /// シーンの起点となるオブジェクト
    /// </summary>
    public class SceneEntry : LifetimeScope{

        [EnumToggleButtons, HideLabel]
        [PropertyOrder(-10)]
        [SerializeField] private SceneType _sceneType = SceneType.MainLevel;

        // カメラ
        [Title("Main Level Settings")]
        [ShowIf("@_sceneType", SceneType.MainLevel)]
        [SerializeField, Indent] Camera _sceneCamera;

        // BGM
        [ShowIf("@_sceneType", SceneType.MainLevel)]
        [SerializeField, Indent] AudioClip _bgmClip;



        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnValidate() {
            if(_sceneCamera is null) {
                _sceneCamera = Camera.main;
            }
        }
#endif
    }
}
