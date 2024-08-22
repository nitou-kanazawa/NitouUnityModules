#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

// [参考]
//  qiita: Unityで独自の設定のUIを提供できるSettingsProviderの紹介と設定ファイルの保存について https://qiita.com/sune2/items/a88cdee6e9a86652137c

namespace nitou.EditorShared {
    using nitou.Settings;

    public class ProjectSettingsProvider : SettingsProvider {

        // 第一階層をProjectにします
        private const string SettingPath = SettingsProviderKey.ProjectSettings + "Runtime";

        private Editor _editor;


        /// ----------------------------------------------------------------------------
        // Public Method

        public ProjectSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords) {}

        public override void OnActivate(string searchContext, VisualElement rootElement) {
            Editor.CreateCachedEditor(ProjectSettingsSO.Instance, null, ref _editor);
        }

        public override void OnGUI(string searchContext) {

            var instance = ProjectSettingsSO.Instance;
            if(instance == null) {
                if (GUILayout.Button("生成する")) {
                    CreateSettings();
                    Editor.CreateCachedEditor(ProjectSettingsSO.Instance, null, ref _editor);
                }
            }

            _editor.OnInspectorGUI();
        }


        /// ----------------------------------------------------------------------------
        // Static Method

        [SettingsProvider]
        public static SettingsProvider CreateProvider() {
            return new ProjectSettingsProvider(SettingPath, SettingsScope.Project, null);
        }

        private static void CreateSettings() {
            var config = ScriptableObject.CreateInstance<ProjectSettingsSO>();
            var parent = "Assets/Resources";
            if (AssetDatabase.IsValidFolder(parent) == false) {
                // Resourcesフォルダが無いことを考慮
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            var assetPath = Path.Combine(parent, Path.ChangeExtension(nameof(ProjectSettingsSO), ".asset"));
            AssetDatabase.CreateAsset(config, assetPath);
        }

    }

}

#endif