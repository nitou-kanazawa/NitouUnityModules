using UnityEngine;

namespace nitou.Credit{

    [CreateAssetMenu(
        fileName = "Credit_Image", 
        menuName = AssetMenu.Prefix.CreditInfo + "Image"
    )]
    public class ImageCredit : CreditData{

        /// <summary>
        /// É^ÉCÉv
        /// </summary>
        public override CreditType Type => CreditType.Image;

    }
}
