using System.Collections.Generic;

namespace nitou.LevelActors.Interfaces.Core {

    /// <summary>
    /// �D��x�Ɋ�Â��ăI�u�W�F�N�g���Ĕz�u����C���^�[�t�F�[�X�D
    /// </summary>
    public interface IPriority<T> 
        where T : class, IPriority<T> {

        /// <summary>
        /// �D��x�D
        /// </summary>
        int Priority { get; }
    }


    /// <summary>
    /// <see cref="IPriority{T}"/>�^�̊g�����\�b�h�W
    /// </summary>
    public static class PriorityExtensions {

        /// <summary>
        /// �ł������D��x�����N���X�𒊏o����B
        /// �D��x��0�ȉ��̃N���X�͑��݂��Ȃ����̂Ƃ��Ĉ����B
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