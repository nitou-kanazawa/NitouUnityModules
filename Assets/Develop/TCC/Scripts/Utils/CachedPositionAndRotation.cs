using UnityEngine;

namespace nitou {

    /// <summary>
    /// <see cref="Transform"/>の位置姿勢のキャッシュ
    /// </summary>
    public readonly struct CachedPositionAndRotation {

        public readonly Vector3 Position { get; }
        public readonly Quaternion Rotation { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CachedPositionAndRotation(Vector3 position, Quaternion rotation) {
            Position = position;
            Rotation = rotation;
        }

        // 静的メソッドでTransformからキャッシュデータを生成
        public static CachedPositionAndRotation FromTransform(Transform transform) {
            return new CachedPositionAndRotation(transform.position, transform.rotation);
        }

        /// <summary>
        /// キャッシュされた値でTransformを復元
        /// </summary>
        public void ApplyTo(Transform transform) {
            transform.position = Position;
            transform.rotation = Rotation;
        }
    }

}

