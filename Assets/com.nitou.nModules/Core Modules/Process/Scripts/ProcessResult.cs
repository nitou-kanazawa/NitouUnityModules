
namespace nitou.GameSystem {

    /// <summary>
    /// 結果データの基底クラス
    /// </summary>
    public abstract class ProcessResult { }

    /// <summary>
    /// 成功時の結果データ
    /// </summary>
    public class CompleteResult : ProcessResult { }

    /// <summary>
    /// キャンセル時の結果データ
    /// </summary>
    public class CancelResult : ProcessResult { }
}
