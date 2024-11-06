#if UNITY_EDITOR
using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelObjects.Editor{
    using nitou.DesignPattern;

    /// <summary>
    /// プリミティブ形状のメッシュプレハブを管理するデータベース
    /// </summary>
    [CreateAssetMenu(
        fileName = "EnviormentModelDatabase",
        menuName = AssetMenu.Prefix.ScriptableObject + "Editor/Enviorment Mesh List"
    )]
    public class EnviormentModelDatabase : SingletonScriptableObject<EnviormentModelDatabase> {

        /// ----------------------------------------------------------------------------
        #region Field

        [FoldoutGroup("Basic"), Indent]
        [LabelText("Floor")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _floor;

        [FoldoutGroup("Basic"), Indent]
        [LabelText("wall")]
        [PreviewField, AssetsOnly]
        [SerializeField] GameObject _wall;

        #endregion


    }
}
#endif