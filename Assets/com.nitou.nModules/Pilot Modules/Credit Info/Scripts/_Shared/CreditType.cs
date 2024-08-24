using System;

namespace nitou.Credit {

    /// <summary>
    /// クレジットの種類
    /// </summary>
    public enum CreditType {
        
        /// <summary>
        /// フォント
        /// </summary>
        Font,

        /// <summary>
        /// 画像
        /// </summary>
        Image,

        /// <summary>
        /// サウンド
        /// </summary>
        Sound,

        /// <summary>
        /// モデル
        /// </summary>
        Model,

        /// <summary>
        /// スクリプト（OSS）
        /// </summary>
        Script,    
    }


    public static class CreditTypeUtility {

        /// <summary>
        /// 指定したクレジットの型情報を取得する
        /// </summary>
        public static Type GetClassType(this CreditType type) {
            return type switch {
                CreditType.Font => typeof(FontCredit),
                CreditType.Image => typeof(ImageCredit),
                CreditType.Sound => typeof(SoundCredit),
                CreditType.Model => typeof(ModelCredit),
                CreditType.Script => typeof(ScriptCredit),
                _ => throw new NotImplementedException()
            };

        }
    }

}