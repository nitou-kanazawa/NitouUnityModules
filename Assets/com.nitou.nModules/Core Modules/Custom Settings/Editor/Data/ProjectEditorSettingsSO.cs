#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

namespace nitou.EditorShared {
    using nitou.Inspector;

    /// <summary>
    /// Editorで参照するプロジェクト固有の設定データ
    /// </summary>
    [UnityEditor.FilePath(
        "ProjectSettings/MyProjectEditorSettings.asset",
        UnityEditor.FilePathAttribute.Location.ProjectFolder
    )]
    public class ProjectEditorSettingsSO : ScriptableSingleton<ProjectEditorSettingsSO> {

        [Title("Hierarchy")]
        [SerializeField, Indent] HierarchySettings _hierarchySettings;

        [Title("Project Window")]
        [SerializeField, Indent] ProjectWindowSettings _projectWindowSettings;


        public HierarchySettings Hierarchy => _hierarchySettings;

        public void Save() => Save(true);
    }


    /// ----------------------------------------------------------------------------
    #region Hierarchy Settings

    /// <summary>
    /// ヒエラルキーに関する設定データ
    /// </summary>

    [Serializable]
    public class HierarchySettings {

        /// <summary>
        /// Specify how to handle HierarchyObject at runtime.
        /// </summary>
        public enum HierarchyObjectMode {
            /// <summary>
            /// 通常オブジェクトとして扱う
            /// </summary>
            None = 0,
            /// <summary>
            /// play modeで削除する
            /// </summary>
            RemoveInPlayMode = 1,
            /// <summary>
            /// ビルド時に削除する
            /// </summary>
            RemoveInBuild = 2
        }

        //[LabelText("Dammy Object Mode")]
        [SerializeField] HierarchyObjectMode _hierarchyObjectMode = HierarchyObjectMode.RemoveInBuild;
        [SerializeField] bool _showToggles;
        [SerializeField] bool _showComponentIcons;

        // プロパティ
        public HierarchyObjectMode Mode => _hierarchyObjectMode;
        public bool ShowToggles => _showToggles;
        public bool ShowComponentIcons => _showComponentIcons;
    }
    #endregion


    /// ----------------------------------------------------------------------------
    #region Project Window Settings

    [Serializable]
    public class ProjectWindowSettings {
        public int test;
    }
    #endregion

}
#endif