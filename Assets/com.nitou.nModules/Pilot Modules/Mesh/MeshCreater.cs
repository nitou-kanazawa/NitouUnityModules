using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nitou {

    // [NOTE] OSS等に切り替えれるようにしておく

    /// <summary>
    /// メッシュ生成に関する汎用ライブラリ
    /// </summary>
    public static class MeshCreater {

        private const float MIN_VALUE = 0.01f;

        /// <summary>
        /// くさび形メッシュのパラメータ
        /// </summary>
        public struct WedgeMeshParameter {
            // 基本
            public float distance;
            public float height;
            public float angle;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public WedgeMeshParameter(float distance = 10f, float height = 1f, float angle = 30f) {
                this.distance = Mathf.Max(distance, MIN_VALUE);
                this.height = Mathf.Max(height, MIN_VALUE);
                this.angle = angle;
            }
        }

        /// <summary>
        /// くさび形のメッシュを生成する
        /// </summary>
        public static Mesh CreateWedgeMesh(WedgeMeshParameter meshParam) {

            // パラメータ
            float distance = meshParam.distance;
            float height = meshParam.height;
            float halfHeight = height / 2;
            float angle = meshParam.angle;

            int segments = 10;
            int numTriangles = (segments * 4) + 2 + 2;   // ※各セグメントはtop,bottom,far1,far2の4面
            int numVerticles = numTriangles * 3;

            var vertices = new Vector3[numVerticles];

            // ------

            // 底面の頂点座標
            Vector3 bottomCenter = Vector3.down * halfHeight;
            Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * (Vector3.forward * distance) + (Vector3.down * halfHeight);
            Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * (Vector3.forward * distance) + (Vector3.down * halfHeight);

            // 上面の頂点座標
            Vector3 topCenter = bottomCenter + (Vector3.up * height);
            Vector3 topRight = bottomRight + (Vector3.up * height);
            Vector3 topLeft = bottomLeft + (Vector3.up * height);

            int index = 0;

            // left side
            vertices[index++] = bottomCenter;
            vertices[index++] = bottomLeft;
            vertices[index++] = topLeft;

            vertices[index++] = topLeft;
            vertices[index++] = topCenter;
            vertices[index++] = bottomCenter;

            // right side
            vertices[index++] = bottomCenter;
            vertices[index++] = topCenter;
            vertices[index++] = topRight;

            vertices[index++] = topRight;
            vertices[index++] = bottomRight;
            vertices[index++] = bottomCenter;


            float currentAngle = -angle;
            float deltaAngle = (angle * 2) / segments;
            for (int i = 0; i < segments; i++) {
                // 頂点の再計算
                bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * (Vector3.forward * distance) + (Vector3.down * halfHeight);
                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * (Vector3.forward * distance) + (Vector3.down * halfHeight);

                topRight = bottomRight + (Vector3.up * height);
                topLeft = bottomLeft + (Vector3.up * height);

                // far side
                vertices[index++] = bottomLeft;
                vertices[index++] = bottomRight;
                vertices[index++] = topRight;

                vertices[index++] = topRight;
                vertices[index++] = topLeft;
                vertices[index++] = bottomLeft;

                // top
                vertices[index++] = topCenter;
                vertices[index++] = topLeft;
                vertices[index++] = topRight;

                // bottom
                vertices[index++] = bottomCenter;
                vertices[index++] = bottomRight;
                vertices[index++] = bottomLeft;

                // 更新
                currentAngle += deltaAngle;
            }

            // ------

            // メッシュ生成
            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = Enumerable.Range(0, numVerticles).ToArray();
            mesh.RecalculateNormals();
            return mesh;
        }



    }



}
