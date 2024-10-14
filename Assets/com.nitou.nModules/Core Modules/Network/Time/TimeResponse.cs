using System;
using UnityEngine;

namespace nitou.Networking{

    [Serializable]
    public class TimeResponse : HttpResponse{

        public string datetime;  // APIから取得される日時のフォーマット
        public string timezone;
        public string utc_offset;

        // このメソッドでレスポンスデータをパース
        public DateTime GetDateTime() {
            if (DateTime.TryParse(datetime, out DateTime parsedDateTime)) {
                return parsedDateTime;
            }
            return DateTime.MinValue;  // パース失敗時のデフォルト値
        }

        public override string ToString() {
            return $"Timezone: {timezone}, DateTime: {datetime}, UTC Offset: {utc_offset}";
        }
    }
}
