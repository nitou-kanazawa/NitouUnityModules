using UnityEngine;
using Sirenix.OdinInspector;

// [参考]
//  qiita: Unityで独自の設定のUIを提供できるSettingsProviderの紹介と設定ファイルの保存について https://qiita.com/sune2/items/a88cdee6e9a86652137c

namespace nitou.Settings {

    /// <summary>
    /// Runtimeで参照するプロジェクト固有の設定データ
    /// </summary>
    public class ProjectSettingsSO : ScriptableObject {

        #region Singleton
        private static ProjectSettingsSO _instance;
        public static ProjectSettingsSO Instance {
            get {
                if (_instance == null) {
                    _instance = Resources.Load<ProjectSettingsSO>(nameof(ProjectSettingsSO));
                }
                return _instance;
            }
        }
        #endregion


        /// ----------------------------------------------------------------------------

        [Title(" ")]
        [Indent] public bool executeAppLauncher;
        [Indent] public string text;

        [Title("UI")]

        [Indent] public bool s;

        [SerializeField] Vector2 _referenceResolution = new Vector2(1920, 1080);
        
        [SerializeField] int _screenCanvasSortingOrder = 10;
        [SerializeField] int _overlayCanvasSortingOrder = 100;



        /// ----------------------------------------------------------------------------

        /// <summary>
        /// 基準解像度
        /// </summary>
        public Vector2 ReferenceResolution => _referenceResolution;

        /// <summary>
        /// 通常キャンバスの描画順
        /// </summary>
        public int ScreenCanvasSortingOrder => _screenCanvasSortingOrder;

        /// <summary>
        /// オーバーレイキャンバスの描画順
        /// </summary>
        public int OverlayCanvasSortingOrder => _overlayCanvasSortingOrder;
    }

}