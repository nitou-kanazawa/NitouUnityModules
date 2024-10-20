
namespace nitou.Detecor{

    /// <summary>
    /// センサで検出可能なオブジェクト
    /// </summary>
    public interface ISensorDetectable {

        /// <summary>
        /// 検出範囲に入った時の処理
        /// </summary>
        internal void OnEnter();

        /// <summary>
        /// 検出範囲から出た時の処理
        /// </summary>
        internal void OnExit();
    }
}
