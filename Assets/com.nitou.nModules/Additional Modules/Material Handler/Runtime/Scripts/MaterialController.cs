using UnityEngine;
using Sirenix.OdinInspector;

// [参考]
//  qiita: 3Dオブジェクトの残像処理 https://qiita.com/madoramu_f/items/fada99645cd03fd7f515
//  UnityIndies: マテリアル、理解してないとすぐにメモリリーク https://www.create-forever.games/unity-material-memory-leak/

namespace nitou.MaterialControl {

    /// <summary>
    /// マテリアルの操作を行うコンポーネント
    /// </summary>
    public abstract class MaterialController<T> : MonoBehaviour, INormalizedValueTicker
        where T : MaterialHandler {

        [Title(IKey.TARGET)]

        [DisableInPlayMode]
        [SerializeField, Indent] Renderer _renderer = null;
        [ReadOnly]
        [SerializeField, Indent] Shader _shader = null;

        protected T _handler = null;

        [Title(IKey.PROPERITY)]
        [SerializeField, Indent] NormalizedValue _rate;

        /// <summary>
        /// 対象マテリアル
        /// </summary>
        public T Handler => _handler;

        /// <summary>
        /// マテリアル変数を一括操作するためのプロパティ
        /// </summary>
        public NormalizedValue Rate {
            get => _rate;
            set {
                _rate = value;
                if (_handler != null) {
                    _handler.Rate = _rate;
                }
            }
        }


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method 

        private void Awake() {
            _handler = CreateHandler(_shader);
            _renderer.SetSharedMaterial(_handler);

            _handler.Rate = _rate;
        }

        private void OnDestroy() {
            _handler?.Dispose();
        }

        private void OnValidate() {
            if (_renderer == null) _renderer = gameObject.GetComponent<Renderer>();
            if (_handler != null) {
                _handler.Rate = _rate;
            }
            if (_shader == null) _shader = FindShader();
        }


        /// ----------------------------------------------------------------------------
        // Protected Method 

        /// <summary>
        /// ハンドラーを生成する
        /// </summary>
        protected abstract T CreateHandler(Shader shader);

        /// <summary>
        /// 対象シェーダーを取得する
        /// </summary>
        protected abstract Shader FindShader();
    }

}

