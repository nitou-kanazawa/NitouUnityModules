using UnityEngine;
using Sirenix.OdinInspector;

// [参考]
//  qiita: UnityでGameObjectにHideFlagsを設定した時の挙動まとめ https://qiita.com/Shairo/items/df8077452d632e788bc1
//  Hatena: Componentの値をInspectorView上で編集不可能にする https://www.mum-meblog.com/entry/unity-tips/sample-hideflags

namespace nitou.LevelObjects {

    /// <summary>
    /// <see cref="LineRenderer"/>に操作するコンポーネントの基底クラス
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(LineRenderer))]
    public abstract class LineRendererAddOn : MonoBehaviour {

         protected LineRenderer _renderer;

        [Title("Parameters")]
        [SerializeField, Indent] protected Material _material;
        [SerializeField, Indent] protected Color _color = Color.white;
        [SerializeField, Indent] protected float _thickness = 0.05f;

        //[SerializeField, Indent] protected float _dotCount = 12;

        protected virtual bool UseWorldSpace => false;

        private bool _isDirty;


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        protected virtual void OnValidate() {
            _renderer = gameObject.GetOrAddComponent<LineRenderer>();
            _renderer.hideFlags = HideFlags.NotEditable;
            _renderer.material = _material;

            _isDirty = true;
        }

        private void OnDestroy() {
            if (_renderer != null) {
                _renderer.hideFlags = HideFlags.None;
            }
        }

        private void Update() {
            //if (Application.isPlaying) return;  // ※実行中は更新しない

            if (_isDirty || CheckDirtyFlag()) {
                UpdateLineRenderer();
            }
        }


        /// ----------------------------------------------------------------------------
        // Protected Method

        /// <summary>
        /// <see cref="LineRenderer"/>を更新する
        /// </summary>
        protected virtual void UpdateLineRenderer() {
            _isDirty = false;
            if (_renderer == null) return;

            // 
            _renderer.SetColor(_color);
            _renderer.SetConstantWidth(_thickness);
            _renderer.useWorldSpace = UseWorldSpace;
            _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            // 頂点の設定
            var points = CreateVertices();
            _renderer.positionCount = points.Length;
            _renderer.SetPositions(points);
        }

        /// <summary>
        /// 頂点を生成する
        /// </summary>
        protected abstract Vector3[] CreateVertices();

        /// <summary>
        /// 描画を更新するかを確認する
        /// </summary>
        protected abstract bool CheckDirtyFlag();
    }
}
