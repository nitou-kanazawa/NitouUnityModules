using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

namespace nitou.GameSystem {
    using nitou.DesignPattern;
    using nitou.SceneSystem;

    /// <summary>
    /// ゲームの設定情報を持つスクリプタブルオブジェクトs
    /// </summary>
    [CreateAssetMenu(
        fileName = "ApplicationConfig",
        menuName = AssetMenu.Prefix.ScriptableObject + "Framwork/App Config"
    )]
    public class AppConfigSO : SingletonScriptableObject<AppConfigSO> {

        [SerializeField, Indent] int _targetFrameRate = 60;

        /// ----------------------------------------------------------------------------
        #region MyRegion

        // Input
        [TitleGroup(IKey.INPUT)]
        [SerializeField, Indent] InputActionAsset _uiInput;
        
        [TitleGroup(IKey.INPUT)]
        [SerializeField, Indent] InputActionAsset _gameplayInput;


        // Event Channel
        [TitleGroup("Event Channel")]
        [SerializeField, Indent] GameEventChannel _eventChannel;


        // Scene
        [TitleGroup("Scene")]
        [SerializeField, Indent] List<SceneObject> _initialLoadedScenes = new();

        [Title("Game Booting")]
        [SerializeField, Indent] string _RootScene = "RootScene";


        [SerializeField] bool _audioManager;
        [SerializeField] bool _uiManager;

        #endregion

        
        /// ----------------------------------------------------------------------------
        
        /// <summary>
        /// 基準解像度
        /// </summary>
        public Vector2 ReferenceResolution => _referenceResolution;
        [SerializeField] Vector2 _referenceResolution = new Vector2(1920,1080);

        public int OverlayCanvasSortingOrder => _overlayCanvasSortingOrder;
        [SerializeField] int _overlayCanvasSortingOrder = 100;


        /// ----------------------------------------------------------------------------
        #region MyRegion

        // Properity      

        public int TargetFrameRate => _targetFrameRate;

        public InputActionAsset UiInput => _uiInput;
        public InputActionAsset PlayerInput => _gameplayInput;

        public GameEventChannel EventChannel => _eventChannel;

        /// <summary>
        /// 開始時に読み込まれるシーンのリスト
        /// </summary>
        public IReadOnlyList<SceneObject> InitialLoadedScenes => _initialLoadedScenes;


        public string RootSceneName => _RootScene;
        
        #endregion

    }
}
