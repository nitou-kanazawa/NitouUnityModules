using System;
using System.IO;
using System.Text;
using UnityEngine;

// [参考]
//  Hatena: Jsonファイルを利用したセーブ機能の実装 https://kiironomidori.hatenablog.com/entry/unity_save_json

namespace nitou.SaveSystem {

    public class BasicDataService : DataServiceBase {

        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BasicDataService(bool encrypted) : base(encrypted){}


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// データを保存する
        /// </summary>
        protected override string ToJson<T>(T data) => JsonUtility.ToJson(data, true);

        /// <summary>
        /// データを読み込む
        /// </summary>
        protected override T FromJson<T>(string json) => JsonUtility.FromJson<T>(json);
    }
}
