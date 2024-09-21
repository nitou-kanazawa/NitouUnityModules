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
    /// <see cref="SceneType"/>型の基本的な拡張メソッド集
    /// </summary>
    public static class SceneTypeExtensions {

        /// <summary>
        /// レベルかどうか
        /// </summary>
        public static bool IsLevel(this SceneType type) => 
            (type == SceneType.MainLevel) || (type == SceneType.SubLevel);

        /// <summary>
        /// タイプに対応したカラーへ変換する
        /// </summary>
        public static Color ToColor(this SceneType type) {
            return type switch {
                SceneType.MainLevel => Colors.Orange,
                SceneType.SubLevel => Colors.Cyan,
                SceneType.Other => Colors.Gray,
                _ => throw new System.NotImplementedException()
            };
        }
    }

}
