using UnityEngine;

namespace nitou.Credit{

    [CreateAssetMenu(
        fileName = "Credit_Sound", 
        menuName = AssetMenu.Prefix.CreditInfo + "Sound"
    )]
    public class SoundCredit : CreditData{

        /// ----------------------------------------------------------------------------
        // Properity

        /// <summary>
        /// É^ÉCÉv
        /// </summary>
        public override CreditType Type => CreditType.Sound;

    }
}
