#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace nitou.LevelObjects.EditorScripts {

    /// <summary>
    /// <see cref="LineRenderer"/>ä÷òAÇÃÉÅÉjÉÖÅ[
    /// </summary>
    public static class LineRendererCreationMenu {

        
        [MenuItem(GameObjectMenu.Prefix.Line + "s")]
        private static void Create(MenuCommand menuCommand) {

            // 
            var obj = new GameObject("Line");
            obj.AddComponent<LineToTarget>();
            obj.SetParentAndAlign(menuCommand.context as GameObject);

            // 
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            Selection.activeObject = obj;
        }

    }
}
#endif