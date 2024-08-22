namespace nitou.SaveSystem{

    /// <summary>
    /// データの保存・読み込みの実処理を担うインターフェース
    /// </summary>
    public interface IDataService{

        /// <summary>
        /// データを保存する
        /// </summary>
        bool SaveData<T>(string key, T data);

        /// <summary>
        /// データを読み込む
        /// </summary>
        T LoadData<T>(string key);
    }
}
