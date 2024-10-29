using UnityEngine;
using nitou.Inspector;
using Sirenix.OdinInspector;

namespace Develop
{
    public class Tester : MonoBehaviour
    {

        public string text1;
        
        [TagSelector]
        public string text2;
        
        [TagSelector(UseDefaultTagFieldDrawer = true)]
        public string text3;


    }
}
