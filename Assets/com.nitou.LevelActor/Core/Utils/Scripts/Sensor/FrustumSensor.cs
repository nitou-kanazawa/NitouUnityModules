using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.LevelObjects{

    public class FrustumSensor : MonoBehaviour{

        // ÉpÉâÉÅÅ[É^
        public float _distance = 5f;
        public float _height = 1.0f;
        public float _angle = 30f;


        [SerializeField] new Camera camera;


        void Start() {
            // Calculate the planes from the main camera's view frustum
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

            // Create a "Plane" GameObject aligned to each of the calculated planes
            for (int i = 0; i < 6; ++i) {
                GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
                p.name = "Plane " + i.ToString();
                p.transform.position = -planes[i].normal * planes[i].distance;
                p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
            }
        }

        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private Mesh _mesh;

        [SerializeField] Color meshColor = Colors.Cyan;

        private void OnValidate() {

            //GeometryUtility

            //var param = new MeshCreater.WedgeMeshParameter(_distance, _height, _angle);
            //_mesh = MeshCreater.CreateFrustumMesh(param);
        }



        private void OnDrawGizmos() {
            if ( _mesh == null) return;

            // mesh
            //Gizmos_.DrawMesh(_mesh, transform.position, transform.rotation, meshColor);


        }
#endif

    }


}
