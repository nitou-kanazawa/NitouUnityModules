using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using nitou.LevelObjects;

// [参考]
//  youtube: センサーを使用した Unity の視線チェック [AI #08] https://www.youtube.com/watch?v=znZXmmyBF-o&t=2s

namespace nitou.LevelActors.Sensor {

    [ExecuteAlways]
    public class AISensor : MonoBehaviour, IManualSensor {

        private const string SHAPE_PARAM = "Shape";
        private const string COLLISION_PARAM = "Collision ";

        [FoldoutGroup(SHAPE_PARAM), Indent]
        [Range(0.1f, 20f)]
        [SerializeField] float _distance = 5f;

        [FoldoutGroup(SHAPE_PARAM), Indent]
        [Range(0.1f, 10f)]
        [SerializeField] float height = 1.0f;

        [FoldoutGroup(SHAPE_PARAM), Indent]
        [Range(10, 90)]
        [SerializeField] float _angle = 30;

        // ----- 

        [FoldoutGroup(COLLISION_PARAM), Indent]
        public bool autoSencing = false;

        [FoldoutGroup(COLLISION_PARAM), Indent]
        public int scanFrequency = 30;

        [FoldoutGroup(COLLISION_PARAM), Indent]
        [SerializeField] LayerMask _layers;
        
        [FoldoutGroup(COLLISION_PARAM), Indent]
        [SerializeField] LayerMask _occlusionLayers;


        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<GameObject> Objects => _objects;


        private readonly Collider[] _colliders = new Collider[50];
        private readonly List<GameObject> _objects = new List<GameObject>();
        private int _count;
        private float _scanTimer;

        private float ScanInterval => 1f / scanFrequency;

        private Mesh _mesh;


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method 

        private void Awake() {
            if (_mesh == null) {
                //var param = new MeshCreater.WedgeMeshParameter(_distance, height, _angle);
                //_mesh = MeshCreater.CreateWedgeMesh(param);
            }
        }

        private void Update() {
            if (!autoSencing) return;

            _scanTimer -= Time.deltaTime;
            if (_scanTimer < 0) {
                _scanTimer += ScanInterval;
                Scan();
            }
        }


        /// ----------------------------------------------------------------------------
        // Public Method 

        /// <summary>
        /// 
        /// </summary>
        public void Scan() {
            _count = Physics.OverlapSphereNonAlloc(transform.position, _distance, _colliders, _layers, QueryTriggerInteraction.Collide);

            _objects.Clear();
            for (int i = 0; i < _count; i++) {
                if (IsInSight(_colliders[i])) {
                    _objects.Add(_colliders[i].gameObject);
                }
            }
        }

        /// <summary>
        /// 内外判定処理
        /// </summary>
        public bool IsInSight(Collider col) {
            var origin = transform.position;
            var dest = col.bounds.center;
            var direction = dest - origin;

            // 距離
            if (direction.sqrMagnitude > Mathf.Pow(_distance, 2)) return false;

            // 高さ
            var halfHeight = height / 2;
            if (direction.y < -halfHeight || halfHeight < direction.y) return false;

            // 角度
            direction.y = 0f;
            float deltaAbgle = Vector3.Angle(direction, transform.forward);
            if (deltaAbgle > _angle) return false;

            // オクルージョン
            if(Physics.Linecast(origin, dest, _occlusionLayers)) return false;


            return true;
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR

        [SerializeField] Color meshColor = Colors.Cyan;

        private void OnValidate() {
            //var param = new MeshCreater.WedgeMeshParameter(_distance, height, _angle);
            //_mesh = MeshCreater.CreateWedgeMesh(param);
        }

            

        private void OnDrawGizmosSelected() {
            if (!autoSencing || _mesh == null) return;

            // mesh
            Gizmos_.DrawMesh(_mesh, transform.position, transform.rotation, meshColor);

            // collision
            Gizmos_.DrawWireSphere(transform.position, _distance, Colors.Gray);
            for (int i = 0; i < _count; i++) {
                Gizmos_.DrawWireSphere(_colliders[i].transform.position, 0.2f, Colors.Green);
            }

            foreach (var obj in _objects) {
                Gizmos.DrawLine(transform.position, obj.transform.position);
                Gizmos_.DrawSphere(obj.transform.position, 0.2f, Colors.Green);
            }
        }
#endif

    }
}
