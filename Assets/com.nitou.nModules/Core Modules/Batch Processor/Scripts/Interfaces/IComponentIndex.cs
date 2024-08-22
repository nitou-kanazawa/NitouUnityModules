
namespace nitou.BachProcessor{

    /// <summary>
    /// バッチ処理の対象コンポーネントのインターフェース
    /// </summary>
    public interface IComponentIndex{

        /// <summary>
        /// Index of the component during batch processing.
        /// </summary>
        int Index { get; set; }
        
        /// <summary>
        /// True if the component is active during batch processing.
        /// </summary>
        bool IsRegistered { get; }
    }
}