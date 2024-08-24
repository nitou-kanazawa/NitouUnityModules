using UnityEngine;

namespace nitou.GameSystem{

    /// <summary>
    /// アプリケーションの入力を扱うインターフェース
    /// </summary>
    public interface IAppInputHandler : System.IDisposable{

        // Player
        public void EnablePlayer();
        public void DisablePlayer();

        // UI
        public void EnableUI();
        public void DisableUI();

        public void DisableAll();
    }
}
