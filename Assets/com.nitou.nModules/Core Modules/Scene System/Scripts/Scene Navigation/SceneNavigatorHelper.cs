using UnityEngine;
using UnityEngine.SceneManagement;

namespace nitou.SceneSystem{

    public static class SceneNavigatorHelper{

        /// <summary>
        /// 全てのビルドシーンからエントリーポイントを探す
        /// </summary>
        public static (Scene sceneThatContainsEntryPoint, ISceneEntryPoint firstEntryPoint) 
            FindFirstEntryPointInAllScenes() {
            
            Scene sceneThatContainsEntryPoint = default;
            ISceneEntryPoint firstEntryPoint = null;

            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++) {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!scene.TryGetComponentInScene(out ISceneEntryPoint entryPoint, true)) {
                    continue;
                }

                // ※複数のシーンで見つかった場合，
                if (firstEntryPoint != null) {
                    Debug_.LogError("Multiple SceneEntryPoint found.");
                    continue;
                }

                sceneThatContainsEntryPoint = scene;
                firstEntryPoint = entryPoint;
            }

            return (sceneThatContainsEntryPoint, firstEntryPoint);
        }


    }
}
