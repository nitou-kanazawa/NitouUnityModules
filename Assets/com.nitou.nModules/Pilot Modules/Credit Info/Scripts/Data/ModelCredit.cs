using UnityEngine;

namespace nitou.Credit{

    [CreateAssetMenu(
        fileName = "Credit_Model", 
        menuName = AssetMenu.Prefix.CreditInfo + "Model"
    )]
    public class ModelCredit : CreditData {

        /// <summary>
        /// É^ÉCÉv
        /// </summary>
        public override CreditType Type => CreditType.Model;
    }
}
