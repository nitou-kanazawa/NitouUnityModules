using UnityEngine;

namespace nitou {

    /// <summary>
    /// <see cref="Transform"/>�̈ʒu�p���̃L���b�V��
    /// </summary>
    public readonly struct CachedPositionAndRotation {

        public readonly Vector3 Position { get; }
        public readonly Quaternion Rotation { get; }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public CachedPositionAndRotation(Vector3 position, Quaternion rotation) {
            Position = position;
            Rotation = rotation;
        }

        // �ÓI���\�b�h��Transform����L���b�V���f�[�^�𐶐�
        public static CachedPositionAndRotation FromTransform(Transform transform) {
            return new CachedPositionAndRotation(transform.position, transform.rotation);
        }

        /// <summary>
        /// �L���b�V�����ꂽ�l��Transform�𕜌�
        /// </summary>
        public void ApplyTo(Transform transform) {
            transform.position = Position;
            transform.rotation = Rotation;
        }
    }

}

