#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace nitou.SceneSystem.EditorScripts{
    using nitou.SceneSystem.Demo;

    /// <summary>
    /// 
    /// </summary>
    internal static class SceneSystemCreationMenu {
        
        /// <summary>
        /// 
        /// </summary>
        [MenuItem(GameObjectMenu.Prefix.SceneSystem + "Scene Loader")]
        private static void Create(MenuCommand menuCommand) {

            // 
            var obj = new GameObject("Scene Loader");
            obj.AddComponent<SceneLoadComponent>();
            GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);

            // 
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            Selection.activeObject = obj;
        }

    }
}
#endif