using UnityEngine;

namespace nitou.SceneSystem{

    public enum SceneType {

        /// <summary>
        /// プレイヤーが属するメインのレベル
        /// </summary>
        MainLevel,
        
        /// <summary>
        /// 付加的なレベル
        /// </summary>
        SubLevel,

        /// <summary>
        /// その他
        /// </summary>
        Other,
    }


    /// <summary>
    /// <see cref="SceneType"/>を対象とした拡張メソッド
    /// </summary>
    public static class SceneTypeExtensions {

        /// <summary>
        /// レベルかどうか
        /// </summary>
        public static bool IsLevel(this SceneType type) => 
            (type == SceneType.MainLevel) || (type == SceneType.SubLevel);

    }

}
