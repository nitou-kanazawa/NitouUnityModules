using System;
using Cysharp.Threading.Tasks;

namespace nitou.GameSystem {

    /// <summary>
    /// 
    /// </summary>
    public interface IProcess : IDisposable{

        /// <summary>
        /// 終了時の通知
        /// </summary>
        public UniTask<ProcessResult> ProcessFinished { get; }

        /// <summary>
        /// 開始する
        /// </summary>
        public void Run();
        
        /// <summary>
        /// ポーズする
        /// </summary>
        public void Pause();

        /// <summary>
        /// ポーズ解除する
        /// </summary>
        public void UnPause();

        /// <summary>
        /// キャンセルする
        /// </summary>
        public void Cancel(CancelResult cancelResult = null);
    }
}
