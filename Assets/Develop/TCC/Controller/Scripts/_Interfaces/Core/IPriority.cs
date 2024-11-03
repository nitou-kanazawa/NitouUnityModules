using System.Collections.Generic;

namespace nitou.LevelActors.Interfaces.Core {

    /// <summary>
    /// 優先度に基づいてオブジェクトを再配置するインターフェース．
    /// </summary>
    public interface IPriority<T> 
        where T : class, IPriority<T> {

        /// <summary>
        /// 優先度．
        /// </summary>
        int Priority { get; }
    }


    /// <summary>
    /// <see cref="IPriority{T}"/>型の拡張メソッド集
    /// </summary>
    public static class PriorityExtensions {

        /// <summary>
        /// 最も高い優先度を持つクラスを抽出する。
        /// 優先度が0以下のクラスは存在しないものとして扱う。
        /// </summary>
        public static bool GetHighestPriority<T>(this IEnumerable<T> values, out T result) 
            where T : class, IPriority<T> {

            result = null;
            var highestPriority = 0;

            foreach (var value in values) {
                if (highestPriority >= value.Priority)
                    continue;

                result = value;
                highestPriority = value.Priority;
            }

            return highestPriority != 0;
        }
    }
}