using UnityEngine;

namespace nitou.LevelObjects{
    using nitou.Inspector;

    //[ExecuteAlways]
    public class LineToTarget : LineRendererAddOn {

        [Title("Target Components")]
        [SerializeField, Indent]
        private Transform _targetTrans;

        protected override bool UseWorldSpace => true;

        /// <summary>
        /// 頂点を生成する
        /// </summary>
        protected override Vector3[] CreateVertices() {
            if (_targetTrans == null) {
                return new Vector3[0]; // 空の頂点配列を返す
            }

            // 始点と終点の2点を設定
            return new Vector3[] { transform.position, _targetTrans.position };
        }

        /// <summary>
        /// 描画を更新するかを確認する
        /// </summary>
        protected override bool CheckDirtyFlag() => _targetTrans != null;
    }
}
