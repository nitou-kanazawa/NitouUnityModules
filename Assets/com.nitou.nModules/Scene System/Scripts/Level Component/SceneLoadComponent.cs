using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace nitou.SceneSystem.Demo{
    using nitou.RichText;
    using nitou.Inspector;

    /// <summary>
    /// インスペクタで設定したシーンを読み込むコンポーネント
    /// </summary>
    [AddComponentMenu(ComponentMenu.Prefix.Scene + "Scene Loader")]
    public class SceneLoadComponent : MonoBehaviour{

        public SceneObject _nextScene;

        /// <summary>
        /// 設定したシーンを読み込む
        /// </summary>
        [Button]
        public async void LoadScene() {
            if (_nextScene == null) return;

            if (!Application.isPlaying) {
                Debug_.LogError("This can only be used during play mode.");
                return;
            }

            string SceneName = _nextScene;
            if(SceneName == SceneNavigator.GetActiveScene().name) {
                Debug_.LogWarning($"Scene [{SceneName.WithColorTag(Colors.Orange)}] is alredy loaded.");
                return;
            }

            var current = SceneManager.GetActiveScene();
            await SceneNavigator.LoadSceneAsync(_nextScene);
            SceneNavigator.UnLoadSceneAsync(current.name).Forget();
        }

    }
}

#if UNITY_EDITOR
namespace nitou.SceneSystem.Demo.EditorScripts {

    [CustomEditor(typeof(SceneLoadComponent))]
    public class SceneLoadComponentEditor : Editor {

        public override void OnInspectorGUI() {

            var instance = (SceneLoadComponent)target;
            var sceneProperty = serializedObject.FindProperty("_nextScene");




            EditorGUILayout.PropertyField(sceneProperty);
            serializedObject.ApplyModifiedProperties();

            if (!Application.isPlaying) return;

            // LOADボタン
            EditorGUILayout.Space();
            if (GUILayout.Button("Load Scene")) {
                instance.LoadScene();
            }
        }

    }
}
#endif
