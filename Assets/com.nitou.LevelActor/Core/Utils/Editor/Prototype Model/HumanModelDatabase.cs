#if UNITY_EDITOR
using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelObjects.EditorScripts {
    using nitou.DesignPattern;

    public enum HumanType {
        Runner,
        Warrior,
        SD,
    }

    /// <summary>
    /// 人型メッシュプレハブを管理するデータベース
    /// </summary>
    [CreateAssetMenu(
        fileName = "HumanModelDatabase", 
        menuName = AssetMenu.Prefix.ScriptableObject + "Editor/Human Mesh List"
    )]
    public sealed class HumanModelDatabase : SingletonScriptableObject<HumanModelDatabase> ,IModelDatabase {

        /// ----------------------------------------------------------------------------
        // Field

        [FoldoutGroup("Male"), Indent]
        [LabelText("Runner")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _runner;

        [FoldoutGroup("Male"), Indent]
        [LabelText("Warrior")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _warrior;

        [FoldoutGroup("Female"), Indent]
        [LabelText("SD")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _sd;


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// 指定した種類のモデルを取得する
        /// </summary>
        public bool TryGetPrefab(HumanType type, out GameObject meshPrefab) {
            meshPrefab = type switch {

                // Male Meshs
                HumanType.Runner => _runner,
                HumanType.Warrior => _warrior,
                HumanType.SD => _sd,
                _ => null
            };

            return meshPrefab != null;
        }
    }
}
#endif