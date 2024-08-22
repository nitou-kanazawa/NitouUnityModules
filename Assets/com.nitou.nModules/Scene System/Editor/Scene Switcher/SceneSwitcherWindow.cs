#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

// [参考]
//  シーンを簡単に切り替えるエディタ拡張 https://tyfkda.github.io/blog/2021/07/15/unity-scene-switcher.html
//  シーンを切り替えるボタンを表示するエディタ拡張 https://kyoro-s.com/unity-13/
//  Unityがデータを保存するために使うパスについて https://light11.hatenadiary.com/entry/2019/10/07/031405

namespace nitou.SceneSystem.EditorScripts {

    /// <summary>
    /// 編集時のシーン切り替えを容易にするためのウインドウ
    /// </summary>
    public class SceneSwitcherWindow : EditorWindow {

        // [NOTE]
        //  ※マルチシーンエディティングは対応していない

        private List<SceneAsset> _scenes;

        // 描画用
        private Vector2 _scrollPos;

        // データ保存先
        private static string FilePath => $"{Application.persistentDataPath}/_sceneLauncher.sav";


        /// ----------------------------------------------------------------------------
        // EditorWindow Method

        [MenuItem(
            MenuItemName.Prefix.EditorWindow + "Scene Switcher",
            priority = -1001
        )]
        static void Open() {
            GetWindow<SceneSwitcherWindow>("Scene Switcher");
        }

        private void OnEnable() {
            if (_scenes == null) {
                _scenes = new List<SceneAsset>();
                Load();
            }
        }

        private void OnGUI() {
            DrawHeader();
            GuiLine();
            DrawContents();
        }


        /// ----------------------------------------------------------------------------
        // Private Method (Drawing)

        private void DrawHeader() {
            using (new EditorGUILayout.HorizontalScope(GUI.skin.box)) {

                // 指定シーンの追加
                var sceneAsset = EditorGUILayout.ObjectField(null, typeof(SceneAsset), false) as SceneAsset;
                if (sceneAsset != null && !_scenes.Contains(sceneAsset)) {
                    _scenes.Add(sceneAsset);
                    Save();
                }

                // 現在シーンの追加
                if (GUILayout.Button("Add current scene")) {
                    var scene = EditorSceneManager.GetActiveScene();
                    if (scene != null && scene.path != null &&
                        _scenes.Find(s => AssetDatabase.GetAssetPath(s) == scene.path) == null) {

                        // シーンアセットを取得
                        var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                        if (asset != null && !_scenes.Contains(asset)) {
                            _scenes.Add(asset);
                            Save();
                        }
                    }
                }
            }
        }

        private void DrawContents() {

            using var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPos); _scrollPos = scrollView.scrollPosition;

            // シーンリストの表示
            for (var i = 0; i < _scenes.Count; ++i) {

                var scene = _scenes[i];
                using (new EditorGUILayout.HorizontalScope()) {
                    
                    var path = AssetDatabase.GetAssetPath(scene);
                    if (GUILayout.Button("X", GUILayout.Width(20))) {
                        _scenes.Remove(scene);
                        Save();
                        --i;
                    } else {
                        if (GUILayout.Button("O", GUILayout.Width(20))) {
                            EditorGUIUtility.PingObject(scene);
                        }
                        if (GUILayout.Button(i > 0 ? "↑" : "　", GUILayout.Width(20)) && i > 0) {
                            _scenes[i] = _scenes[i - 1];
                            _scenes[i - 1] = scene;
                            Save();
                        }

                        // シーンボタン
                        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
                        if (GUILayout.Button(Path.GetFileNameWithoutExtension(path))) {
                            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {  // ※変更があった場合に保存するかの確認用
                                EditorSceneManager.OpenScene(path);
                            }
                        }
                        EditorGUI.EndDisabledGroup();
                    }
                }
            }
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        /// <summary>
        /// 設定データの保存
        /// </summary>
        private void Save() {
            var guids = new List<string>();
            foreach (var scene in _scenes) {
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(scene, out string guid, out long _)) {
                    guids.Add(guid);
                }
            }

            var content = string.Join("\n", guids.ToArray());
            File.WriteAllText(FilePath, content);
        }

        /// <summary>
        /// 設定データの読み込み
        /// </summary>
        private void Load() {
            _scenes.Clear();
            if (File.Exists(FilePath)) {
                string content = File.ReadAllText(FilePath);
                foreach (var guid in content.Split(new char[] { '\n' })) {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                    if (scene != null)
                        _scenes.Add(scene);
                }
            }
        }


        /// ----------------------------------------------------------------------------
        // Util Method

        private static void GuiLine(int height = 1) {
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(height) });
        }
    }

}
#endif