using UnityEngine;

namespace nitou.LevelObjects {
    using nitou.Inspector;

    [ExecuteAlways]
    [RequireComponent(typeof(LineRenderer), typeof(SphereCollider))]
    public class SphereColliderLine : LineRendererAddOn {

        [Title("Circle Parameters")]
        [SerializeField, Indent] private int _segmentCount = 36; // 円を構成するセグメントの数
        [SerializeField, Indent] Vector3 _offset = new (0, 0.1f, 0);

        private SphereCollider _sphere;
        private Bounds _oldBounds;


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        protected override void OnValidate() {
            base.OnValidate();
            if (_sphere == null) {
                _sphere = gameObject.GetOrAddComponent<SphereCollider>();
            }
        }

        /// ----------------------------------------------------------------------------
        // Protected Method

        /// <summary>
        /// 頂点を生成する
        /// </summary>
        protected override Vector3[] CreateVertices() {
            var radius = _sphere.radius;
            var center = _sphere.center;

            Vector3[] points = new Vector3[_segmentCount + 1];
            float angleStep = 360f / _segmentCount;

            for (int i = 0; i <= _segmentCount; i++) {
                float angle = i * angleStep * Mathf.Deg2Rad;
                points[i] = _offset + new Vector3(
                    x: center.x + Mathf.Cos(angle) * radius, 
                    y: center.y, 
                    z: center.z + Mathf.Sin(angle) * radius);
            }
            return points;
        }

        /// <summary>
        /// ダーティフラグをチェックする
        /// </summary>
        protected override bool CheckDirtyFlag() {
            if (_oldBounds != _sphere.bounds) {
                _oldBounds = _sphere.bounds;
                return true;
            }

            return false;
        }
    }

}
