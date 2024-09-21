using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// [参考]
// 　コガネブログ: Inspector で変数にシーンファイルを設定できるようにする「SceneObject」 https://baba-s.hatenablog.com/entry/2017/11/14/110000

namespace nitou.SceneSystem {

    /// <summary>
    /// インスペクターでシーンファイルを設定できるようにするためのクラス
    /// </summary>
    [Serializable]
    public sealed class SceneObject {

        [SerializeField] string _sceneName;

        // 変換
        public static implicit operator string(SceneObject sceneObject) {
            return sceneObject._sceneName;
        }
        public static implicit operator SceneObject(string sceneName) {
            return new SceneObject() { _sceneName = sceneName };
        }
    }
}


/// ----------------------------------------------------------------------------
#if UNITY_EDITOR
namespace nitou.SceneSystem.EditorScripts {

    [CustomPropertyDrawer(typeof(SceneObject))]
    internal class SceneObjectEditor : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            var sceneObj = GetSceneObject(property.FindPropertyRelative("_sceneName").stringValue);
            var newScene = EditorGUI.ObjectField(position, label, sceneObj, typeof(SceneAsset), false);
            if (newScene == null) {
                var prop = property.FindPropertyRelative("_sceneName");
                prop.stringValue = "";
            } else {
                if (newScene.name != property.FindPropertyRelative("_sceneName").stringValue) {
                    var scnObj = GetSceneObject(newScene.name);
                    if (scnObj == null) {
                        Debug.LogWarning("The scene " + newScene.name + " cannot be used. To use this scene add it to the build settings for the project.");
                    } else {
                        var prop = property.FindPropertyRelative("_sceneName");
                        prop.stringValue = newScene.name;
                    }
                }
            }
        }

        /// <summary>
        /// 対象のシーンアセットを取得する
        /// </summary>
        protected SceneAsset GetSceneObject(string sceneObjectName) {
            if (string.IsNullOrEmpty(sceneObjectName)) return null;

            // BuildSettingsに含まれるシーンから検索
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
                EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
                if (scene.path.IndexOf(sceneObjectName) != -1) {
                    return AssetDatabase.LoadAssetAtPath(scene.path, typeof(SceneAsset)) as SceneAsset;
                }
            }

            Debug.Log("Scene [" + sceneObjectName + "] cannot be used. Add this scene to the 'Scenes in the Build' in the build settings.");
            return null;
        }
    }

}
#endif