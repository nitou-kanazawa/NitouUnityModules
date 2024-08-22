#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneTemplate;

// [参考]
//  LIGHT11: シーンの雛形を作れるScene Template機能の使い方まとめ https://light11.hatenadiary.com/entry/2022/06/08/193509

namespace nitou.SceneSystem.EditorScripts {

    public class ExampleTemplatePipline : ISceneTemplatePipeline {

        /// <summary>
        /// 有効なテンプレートか判定する
        /// </summary>
        public bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset) {
            Debug_.Log($"{nameof(IsValidTemplateForInstantiation)} - sceneTemplateAsset: {sceneTemplateAsset.name}");
            return true;
        }

        /// <summary>
        /// シーンが作成された前のコールバック
        /// </summary>
        public void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName) {
            Debug.Log($"{nameof(BeforeTemplateInstantiation)} - isAdditive: {isAdditive} sceneName: {sceneName}");
        }

        /// <summary>
        /// シーンが作成される後のコールバック
        /// </summary>
        public void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, Scene scene, bool isAdditive, string sceneName) {
            Debug.Log($"{nameof(AfterTemplateInstantiation)} - scene: {scene} isAdditive: {isAdditive} sceneName: {sceneName}");
        }
    }
}

#endif