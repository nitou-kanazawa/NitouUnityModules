using UnityEngine;

namespace nitou.GameSystem {

    /// <summary>
    /// ゲームイベント用のデータクラス
    /// </summary>
    public abstract class GameEventData {

        public static EmptyEventData Empty() {
            return new EmptyEventData();
        }
    }


    public class EmptyEventData : GameEventData { }
}
