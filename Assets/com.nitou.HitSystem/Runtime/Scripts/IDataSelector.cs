
namespace nitou{

    /// <summary>
    /// <see cref="TData"/>型のデータを選択できる
    /// </summary>
    public interface IDataSelector<TData>{
        
        /// <summary>
        /// 選択中のインスタンス
        /// </summary>
        TData Selected { get; }

        bool IsValid { get; }

        /// <summary>
        /// 指定インスタンスを選択する
        /// </summary>
        bool Select(TData data);
    }
}
