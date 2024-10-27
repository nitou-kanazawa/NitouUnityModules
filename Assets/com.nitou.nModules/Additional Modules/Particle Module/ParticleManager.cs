using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

// [参考]
//  Hatena: パーティクル再利用クラス https://www.stmn.tech/entry/2016/03/01/004816 
//  qiita: UniRxのObjectPoolを使ってParticleSystemを管理する https://qiita.com/KeichiMizutani/items/fc22a6037447d840adc2
//  kanのメモ帳: UniRxでオブジェクトプール(ObjectPool)を簡単実装 https://kan-kikuchi.hatenablog.com/entry/UniRx_ObjectPool


namespace nitou.ParticleModule {
    using nitou.DesignPattern;

    /// <summary>
    /// パーティクルを管理するグローバルマネージャ．
    /// ※小さなプロジェクトまたはプロトタイプとしての使用を想定
    /// </summary>
    public class ParticleManager : SingletonMonoBehaviour<ParticleManager> {

        // 各パーティクルプールのリスト
        private List<ParticlePool> _poolList = new ();
        private List<GameObject> _containers = new ();

        // リソース情報
        private const string RESOUCE_PATH = "Particles/World/";     


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        protected override void Awake() {
            if (!base.CheckInstance()) {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        private void OnDestroy() {
            // 破棄されたとき（Disposeされたとき）にObjectPoolを解放する
            ClearList();
            _poolList = null;
            _containers = null;
        }


        /// ----------------------------------------------------------------------------
        // Public Method 

        /// <summary>
        /// 指定した名前のパーティクル再生
        /// ※初めて再生するパーティクルはプール用オブジェクトを生成
        /// </summary>
        public void PlayParticle(string particleName, Vector3 position, Quaternion rotation) {
            //リストから指定した名前のプール用オブジェクトを取得
            ParticlePool pool = _poolList.Where(p => p.ParticleName == particleName).FirstOrDefault();

            // プールが未生成の場合，
            if (pool == null) {
                // 格納用の親オブジェクトを生成 (※デバッグ可視化用)
                var parentObj = new GameObject($"Pool [{particleName}]");
                parentObj.SetParent(this.transform);
                _containers.Add(parentObj);

                // 生成元のオブジェクトを取得
                var prefab = LoadOrCreateOrigin(particleName).GetOrAddComponent<ParticleObject>();

                // プールの生成
                pool = new ParticlePool(parentObj.transform, prefab, particleName);
                _poolList.Add(pool);
            }

            // ObjectPoolから1つ取得
            var effect = pool.Rent();

            // エフェクトを再生し、再生終了したらpoolに返却する
            effect.PlayParticle(position, rotation)
                .Subscribe(__ => { pool.Return(effect); });
        }

        /// <summary>
        /// 指定した名前のパーティクル再生 (オーバーロード)
        /// </summary>
        public void PlayParticle(string particleName, Vector3 position) =>
            PlayParticle(particleName, position, Quaternion.identity);

        /// <summary>
        /// 指定した名前のパーティクル再生
        /// </summary>
        public void PlayParticleWithEvent(string particleName, Vector3 position, Quaternion rotation) {
            //リストから指定した名前のプール用オブジェクトを取得
            ParticlePool pool = _poolList.Where(p => p.ParticleName == particleName).FirstOrDefault();

            // プールが未生成の場合，
            if (pool == null) {
                // 格納用の親オブジェクトを生成 (※デバッグ可視化用)
                var parentObj = new GameObject($"Pool [{particleName}]");
                parentObj.SetParent(this.transform);
                _containers.Add(parentObj);

                // 生成元のオブジェクトを取得
                var prefab = LoadOrCreateOrigin(particleName).GetOrAddComponent<ParticleObject>();

                // プールの生成
                pool = new ParticlePool(parentObj.transform, prefab, particleName);
                _poolList.Add(pool);
            }

            // ObjectPoolから1つ取得
            var effect = pool.Rent();

            // エフェクトを再生し、再生終了したらpoolに返却する
            effect.PlayParticleWithEvent(position, rotation)
                .Subscribe(__ => { pool.Return(effect); });
        }

        /// <summary>
        /// 指定した名前のパーティクル再生 (オーバーロード)
        /// </summary>
        public void PlayParticleWithEvent(string particleName, Vector3 position) =>
            PlayParticleWithEvent(particleName, position, Quaternion.identity);

        /// <summary>
        /// オブジェクトプールのリスト解放
        /// </summary>
        public void ClearList() {

            _poolList.ForEach(p => p.Dispose());
            _poolList.Clear();

            _containers.ForEach(o => o.Destroy());
            _containers.Clear();
        }


        /// ----------------------------------------------------------------------------
        // Private Method 

        /// <summary>
        /// 生成元のオブジェクトをロード
        /// ※ロード失敗時はデフォルトのパーティクルを生成
        /// </summary>
        private GameObject LoadOrCreateOrigin(string particleName) {
            // リソースの読み込み
            var origin = Resources.Load(RESOUCE_PATH + particleName) as GameObject;

            if (origin == null) {   // ----- 失敗した場合，
                // ※ダミーを入れておく
                origin = new GameObject($"Defalut Particle");
                Debug.Log($"[{particleName}]というパーティクルの読み込みに失敗しました");

            } else {                // ----- 成功した場合，
                origin.name = particleName;
            }
            return origin;
        }

    }
}