
namespace nitou{

    /// <summary>
    /// 
    /// </summary>
    public interface IDataContainer<TData> {

        int Count { get; }

        TData First { get; }

        /// <summary>
        /// 要素を追加する
        /// </summary>
        void Add(TData item);
        
        /// <summary>
        /// 要素を削除する
        /// </summary>
        bool Remove(TData item);
        
        /// <summary>
        /// 
        /// </summary>
        void Clear();

        /// <summary>
        /// 要素を含んでいるか確認する
        /// </summary>
        bool Contains(TData item);
    }

}
