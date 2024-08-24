#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace nitou.LevelObjects.EditorScripts {

    /// <summary>
    /// プリミティブ形状に関するメニューコマンド
    /// </summary>
    public static class HumanModelCreationMenu {

        private const string CREATE_HUMAN_MODEL = MenuItemName.Prefix.GameObject + "3D Object/Human/";


        /// ----------------------------------------------------------------------------
        // Public Method (Lowpoly Mesh)

        [MenuItem(CREATE_HUMAN_MODEL + "Runner")]
        public static void Create_Runner(MenuCommand menuCommand) => CreateGameObject(menuCommand, HumanType.Runner);

        [MenuItem(CREATE_HUMAN_MODEL + "Warrior")]
        public static void Create_Warrior(MenuCommand menuCommand) => CreateGameObject(menuCommand, HumanType.Warrior);

        [MenuItem(CREATE_HUMAN_MODEL + "SD")]
        public static void Create_SD(MenuCommand menuCommand) => CreateGameObject(menuCommand, HumanType.SD);


        /// ----------------------------------------------------------------------------
        // Private Method

        /// <summary>
        /// モデルのインスタンスを生成する
        /// </summary>
        private static void CreateGameObject(MenuCommand menuCommand, HumanType type) {

            if (HumanModelDatabase.Instance.TryGetPrefab(type, out var meshObj)) {

                // オブジェクト生成
                var gameObject = GameObject.Instantiate(meshObj);
                gameObject.name = type.ToString();

                // 
                GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);

                // Undo設定
                Undo.RegisterCreatedObjectUndo(gameObject, gameObject.name);

                // 選択状態に設定
                Selection.activeObject = gameObject;
            }
        }
    }
}
#endif