using System;
using UnityEngine;

// [参考]
//  qiita: 3Dオブジェクトの残像処理 https://qiita.com/madoramu_f/items/fada99645cd03fd7f515
//  UnityIndies: マテリアル、理解してないとすぐにメモリリーク https://www.create-forever.games/unity-material-memory-leak/
//  Hatena: Renderer.materialで取得したマテリアルは自分で破棄しないとリークする話 https://light11.hatenadiary.com/entry/2019/11/03/223241

namespace nitou.MaterialControl {

    /// <summary>
    /// マテリアルのプロパティ操作用ラッパークラス
    /// </summary>
    public abstract class MaterialHandler : IDisposable , INormalizedValueTicker{

        protected readonly Shader _shader = null;
        protected Material _material = null;

        private NormalizedValue _rate;

        /// <summary>
        /// マテリアル変数を一括操作するためのプロパティ
        /// </summary>
        public NormalizedValue Rate {
            get => _rate;
            set {
                _rate = value;
                OnRateChanged(_rate);
            }
        }

        /// <summary>
        /// メインカラー
        /// </summary>
        public Color Color {
            get => _material.color;
            set => _material.color = value;
        }


        /// ----------------------------------------------------------------------------
        // Public Method 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MaterialHandler(Shader shader) {
            if (shader == null) throw new ArgumentNullException(nameof(shader));

            // マテリアル生成
            _shader = shader;
            _material = new Material(_shader);
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Dispose() {
            if (_material == null) return;
            GameObject.Destroy(_material);
            _material = null;
        }


        /// ----------------------------------------------------------------------------
        // Public Method (基本操作)

        /// <summary>
        /// レンダラーにマテリアルを適用する
        /// </summary>
        public void OnApplayMaterial(Renderer renderer) {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            renderer.sharedMaterial = _material;
        }

        /// <summary>
        /// テクスチャを設定する
        /// </summary>
        public void SetMainTex(Texture texture) {
            _material.mainTexture = texture;
        }

        /// <summary>
        /// カラーを設定する
        /// </summary>
        public void SetMainColor(Color color) {
            _material.color = color;
        }


        /// ----------------------------------------------------------------------------
        // Protected Method 

        /// <summary>
        /// 一括プロパティが変化したときの処理
        /// </summary>
        protected virtual void OnRateChanged(float rate) { }
    }


    public static partial class RendererExtensions {

        /// <summary>
        /// レンダラーにマテリアルを適用する拡張メソッド
        /// </summary>
        public static void SetSharedMaterial(this Renderer self, MaterialHandler handler) {
            handler.OnApplayMaterial(self);
        }
    }
}