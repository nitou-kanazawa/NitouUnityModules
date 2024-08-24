using UnityEngine;
using Sirenix.OdinInspector;

// [参考]
//  _: とほほのライセンス入門 https://www.tohoho-web.com/ex/license.html 

namespace nitou.Credit {

    [CreateAssetMenu(
        fileName = "Credit_Script",
        menuName = AssetMenu.Prefix.CreditInfo + "Script"
    )]
    public class ScriptCredit : CreditData {

        public enum LicenseFormat {
            MIT,
            BSD,
            Apache,
        }

        /// <summary>
        /// ライセンス形式
        /// </summary>
        [TitleGroup(CreditUtility.BasicGroup), Indent]
        public LicenseFormat license = LicenseFormat.MIT;

        /// <summary>
        /// 発行年
        /// </summary>
        [TitleGroup(CreditUtility.BasicGroup), Indent]
        public int publicationYear = 2000;

        /// <summary>
        /// 制作者
        /// </summary>
        [TitleGroup(CreditUtility.BasicGroup), Indent]
        public string author = "";


        /// <summary>
        /// タイプ
        /// </summary>
        public override CreditType Type => CreditType.Script;


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// 表示テキストへ変換する
        /// </summary>
        public override string ToString() {
            return $"<b>{englishName}</b> / (c) {publicationYear} {author}\n" 
                + $"Released under the {license} license\n"
                + url;
        }
    }
}
