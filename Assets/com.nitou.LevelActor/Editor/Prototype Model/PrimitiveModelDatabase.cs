#if UNITY_EDITOR
using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelObjects.EditorScripts {
    using nitou.DesignPattern;

    public enum PrimitiveType {
        
        // Lowpoly Meshs
        LowpolySphere,
        LowpolyCapsuel,
        LowpolyCylinder,
        LowpolyCone,

        // Reverse Meshs
        ReverseCube,
        ReverseSphere,
        ReverseCylinder,
    }

    /// <summary>
    /// プリミティブ形状のメッシュプレハブを管理するデータベース
    /// </summary>
    [CreateAssetMenu(
        fileName = "PrimitiveModelDatabase", 
        menuName = AssetMenu.Prefix.ScriptableObject + "Editor/Primitive Mesh List"
    )]
    public sealed class PrimitiveModelDatabase : SingletonScriptableObject<PrimitiveModelDatabase> , IModelDatabase{

        /// ----------------------------------------------------------------------------
        #region Field

        [FoldoutGroup("Lowpoly"), Indent]
        [LabelText("Sphere")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _lowpolySphere;

        [FoldoutGroup("Lowpoly"), Indent]
        [LabelText("Capsuel")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _lowpolyCapsuel;

        [FoldoutGroup("Lowpoly"), Indent]
        [LabelText("Cylinder")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _lowpolyCylinder;

        [FoldoutGroup("Lowpoly"), Indent]
        [LabelText("Cone")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _lowpolyCone;

        // ---

        [FoldoutGroup("Reverse"), Indent]
        [LabelText("Cube")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _ReverseCube;

        [FoldoutGroup("Reverse"), Indent]
        [LabelText("Sphere")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _ReverseSphere;

        [FoldoutGroup("Reverse"), Indent]
        [LabelText("Cylinder")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _ReverseCylinder;
        #endregion


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// 指定した種類のモデルを取得する
        /// </summary>
        public bool TryGetPrefab(PrimitiveType type, out GameObject meshPrefab) {
            meshPrefab = type switch {
                
                // Lowpoly Meshs
                PrimitiveType.LowpolySphere => _lowpolySphere,
                PrimitiveType.LowpolyCapsuel => _lowpolyCapsuel,
                PrimitiveType.LowpolyCylinder => _lowpolyCylinder,
                PrimitiveType.LowpolyCone => _lowpolyCone,

                // Reverse Meshs
                PrimitiveType.ReverseCube => _ReverseCube,
                PrimitiveType.ReverseSphere => _ReverseSphere,
                PrimitiveType.ReverseCylinder => _ReverseCylinder,
                _ => null
            };

            return meshPrefab != null;
        }
    }
}
#endif