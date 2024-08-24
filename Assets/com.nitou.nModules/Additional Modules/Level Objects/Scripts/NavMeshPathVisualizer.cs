using UnityEngine;
using UnityEngine.AI;

// [参考]
//  _: NavMeshAgentで、これから通る経路を描画する https://indie-du.com/entry/2016/05/21/080000#google_vignette

namespace nitou.LevelObjects{

    /// <summary>
    /// NavMeshPathを可視化する簡易コンポーネント
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class NavMeshPathVisualizer : MonoBehaviour{

        // 描画用コンポーネント
        private LineRenderer _lineRenderer;


        /// ----------------------------------------------------------------------------
        // MonoBehaviour

        private void Awake() {
            _lineRenderer = gameObject.GetComponent<LineRenderer>();

            // 非表示状態に設定
            _lineRenderer.enabled = false;
            this.enabled = false;
        }

        private void OnEnable() {
            _lineRenderer.enabled = true;
        }

        private void OnDisable() {
            _lineRenderer.enabled = false;
        }


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// ラインレンダラーにパスを設定する
        /// </summary>
        public void SetPath(NavMeshPath path, bool showLine = true) {
            Error.ArgumentNullException(path);

            // 表示状態
            this.enabled = showLine;

            // 経路の更新
            _lineRenderer.SetPositions(path.corners);
            //_lineRenderer.SetColor(GetPathColor(path.status));
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        /// <summary>
        /// パスの状態に応じた描画色を取得する
        /// </summary>
        private Color GetPathColor(NavMeshPathStatus pathStatus) => pathStatus switch {
            NavMeshPathStatus.PathComplete => Colors.Green,
            NavMeshPathStatus.PathPartial=> Colors.Yellow,
            NavMeshPathStatus.PathInvalid => Colors.Red,
            _=> throw new System.NotImplementedException()
        };

    }
}
