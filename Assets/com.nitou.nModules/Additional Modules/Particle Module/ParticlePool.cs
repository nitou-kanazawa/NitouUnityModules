using UniRx;
using UniRx.Toolkit;
using UnityEngine;

// [参考]
//  Hatena: パーティクル再利用クラス https://www.stmn.tech/entry/2016/03/01/004816 
//  qiita: UniRxのObjectPoolを使ってParticleSystemを管理する https://qiita.com/KeichiMizutani/items/fc22a6037447d840adc2
//  kanのメモ帳: UniRxでオブジェクトプール(ObjectPool)を簡単実装 https://kan-kikuchi.hatenablog.com/entry/UniRx_ObjectPool

namespace nitou.ParticleModule {

    /// <summary>
    /// パーティクルのオブジェクトプール
    /// </summary>
    public sealed class ParticlePool : ObjectPool<ParticleObject> {

        private ParticleObject _prefab;                 // プレハブ
        private readonly Transform _parentTransform;    // 複製したオブジェクトの親

        /// <summary>
        /// パーティクル名
        /// </summary>
        public string ParticleName => _particleName;
        private readonly string _particleName;


        /// ----------------------------------------------------------------------------
        // Public Method 

        /// コンストラクタ
        public ParticlePool(Transform transform, ParticleObject origin, string particleName = "") {
            this._parentTransform = transform;
            this._prefab = origin;
            this._particleName = particleName;
        }


        /// ----------------------------------------------------------------------------
        // Override Method 

        protected override ParticleObject CreateInstance() {
            if (_prefab == null) SetDefalutPrefab();
            return Object.Instantiate(_prefab, _parentTransform, true);
        }

        protected override void OnBeforeRent(ParticleObject instance) {
            Debug.Log($"{instance.name}がプールから取り出されました");
            base.OnBeforeRent(instance);
        }

        protected override void OnBeforeReturn(ParticleObject instance) {
            Debug.Log($"{instance.name}がプールに戻されました");
            base.OnBeforeReturn(instance);
        }

        protected override void OnClear(ParticleObject instance) {
            base.OnClear(instance);
        }


        /// ----------------------------------------------------------------------------
        // Private Method 

        /// <summary>
        /// デフォルトのパーティクルを登録
        /// </summary>
        private void SetDefalutPrefab() {
            var obj = new GameObject($"Defalut Particle");
            var particle = obj.AddComponent<ParticleSystem>();
            _prefab = obj.AddComponent<ParticleObject>();
        }

    }

}












//// プールからオブジェクトを取得する前に実行される
//protected override void OnBeforeRent(ParticleObject instance) {
//    Debug.Log($"{instance.name}がプールから取り出されました");
//    base.OnBeforeRent(instance);
//}

//// オブジェクトがプールに戻る前に実行される
//protected override void OnBeforeReturn(ParticleObject instance) {
//    Debug.Log($"{instance.name}がプールに戻されました");
//    base.OnBeforeReturn(instance);
//}