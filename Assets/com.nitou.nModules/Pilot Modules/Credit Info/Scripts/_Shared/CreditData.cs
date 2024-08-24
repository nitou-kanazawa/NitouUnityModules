using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace nitou.Credit {

    [InlineEditor]
    public abstract class CreditData : ScriptableObject {

        /// ----------------------------------------------------------------------------
        // Field
        
        
        /// <summary>
        /// プロジェクトで使用しているかどうか
        /// </summary>
        [TitleGroup(CreditUtility.State), Indent]
        public bool isUsed;

        /// <summary>
        /// コンテンツ名
        /// </summary>
        [LabelText("Name")]
        [TitleGroup(CreditUtility.BasicGroup), Indent]
        [OnValueChanged("OnNameChange")]
        public string englishName = "unknown";

        /// <summary>
        /// URL
        /// </summary>
        [TitleGroup(CreditUtility.BasicGroup), Indent]
        public string url;

        /// <summary>
        /// コンテンツ名
        /// </summary>
        [TitleGroup(CreditUtility.AdditionalGroup), Indent]
        public string japaneseName;

        /// <summary>
        /// メモ書き
        /// </summary>
        [HideLabel]
        [TextArea(4,10)]
        [TitleGroup("Memo")]
        public string description;


        /// ----------------------------------------------------------------------------
        // Properity

        /// <summary>
        /// タイプ
        /// </summary>
        public abstract CreditType Type { get; }


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// 表示テキストへ変換する
        /// </summary>
        public override string ToString() {
            return $"<b>{japaneseName}</b>\n" + url;
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnNameChange() {
            if (englishName.IsNullOrWhiteSpace()) {
                englishName = "unknown";
            }

            // ファイル名を更新
            var path = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(path, $"Credit_{Type.ToString()}_{englishName}");
        }
#endif
    }

}