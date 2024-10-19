using UnityEngine;

namespace nitou.Detecor {

    /// <summary>
    /// <see cref="Collider"/>の管理者であることを示すインターフェース
    /// </summary>
    public interface IColliderOwner{

        /// <summary>
        /// Checks whether a collider belongs to the character.
        /// </summary>
        public bool IsOwnCollider(Collider col);

    }
}
