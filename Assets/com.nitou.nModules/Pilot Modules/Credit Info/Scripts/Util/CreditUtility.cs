using System.Collections.Generic;
using System.Text;

namespace nitou.Credit {

    public static class CreditUtility {

        public const string BasicGroup = "Basic Info";

        public const string AdditionalGroup = "Additional Info";

        public const string State = "State";


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// ƒŠƒXƒg—v‘f‚ð•¶Žš—ñ‚É•ÏŠ·‚·‚é
        /// </summary>
        public static string Convert(this IReadOnlyList<CreditData> list) {

            var sb = new StringBuilder();

            foreach(var data in list) {
                sb.Append($"{data} \n\n");
            }
            return sb.ToString();
        }

    }
}
