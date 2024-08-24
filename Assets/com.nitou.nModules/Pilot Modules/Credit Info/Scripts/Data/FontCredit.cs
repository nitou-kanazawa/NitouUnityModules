using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.Credit {

    [CreateAssetMenu(
        fileName = "Credit_Font", 
        menuName = AssetMenu.Prefix.CreditInfo + "Font"
    )]
    public class FontCredit : CreditData {

        /// <summary>
        /// 権利者名
        /// </summary>
        [TitleGroup(CreditUtility.AdditionalGroup), Indent]
        public string authorName;


        /// <summary>
        /// タイプ
        /// </summary>
        public override CreditType Type => CreditType.Font;


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// 表示テキストへ変換する
        /// </summary>
        public override string ToString() {
            return $"<b>{japaneseName}</b>  by {authorName} 様\n" + url;
        }

    }

}