using System;
using UniRx;
using UnityEngine;

// [参考]
//  Hatena: パーティクル再利用クラス https://www.stmn.tech/entry/2016/03/01/004816 
//  qiita: UniRxのObjectPoolを使ってParticleSystemを管理する https://qiita.com/KeichiMizutani/items/fc22a6037447d840adc2
//  kanのメモ帳: UniRxでオブジェクトプール(ObjectPool)を簡単実装 https://kan-kikuchi.hatenablog.com/entry/UniRx_ObjectPool

namespace nitou.ParticleModule {

    /// <summary>
    /// パーティクルを再生するためのラッパーコンポーネント
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    [DisallowMultipleComponent]
    public sealed class ParticleObject : MonoBehaviour {

        private ParticleSystem particle;


        /// ----------------------------------------------------------------------------
        // LifeCycle Events 

        private void Awake() {
            particle = GetComponent<ParticleSystem>();
        }


        /// ----------------------------------------------------------------------------
        // Public Method 

        /// <summary>
        /// パーティクルを再生する
        /// </summary>
        public IObservable<Unit> PlayParticle(Vector3 position) {
            transform.position = position;
            particle.Play();

            // ParticleSystemのstartLifetimeに設定した秒数が経ったら終了通知
            return Observable.Timer(TimeSpan.FromSeconds(particle.main.startLifetimeMultiplier))
                .ForEachAsync(_ => particle.Stop());
        }

        /// <summary>
        /// パーティクルを再生する
        /// </summary>
        public IObservable<Unit> PlayParticle(Vector3 position, Quaternion rotation) {
            transform.position = position;
            transform.rotation = rotation;
            particle.Play();

            // ParticleSystemのstartLifetimeに設定した秒数が経ったら終了通知
            return Observable.Timer(TimeSpan.FromSeconds(particle.main.startLifetimeMultiplier))
                .ForEachAsync(_ => particle.Stop());
        }

        /// <summary>
        /// パーティクルを再生する(※イベント実行)
        /// </summary>
        public IObservable<Unit> PlayParticleWithEvent(Vector3 position) {
            transform.position = position;
            particle.Play();

            var particleEvent = GetComponent<IParticleLifeCycleEvent>();
            particleEvent?.OnPlayed();  // ※開始イベント

            // ParticleSystemのstartLifetimeに設定した秒数が経ったら終了通知
            return Observable.Timer(TimeSpan.FromSeconds(particle.main.startLifetimeMultiplier))
                .ForEachAsync(_ => { 
                    particle.Stop();
                    particleEvent?.OnStopped();    // ※終了イベント
                });
        }

        /// <summary>
        /// パーティクルを再生する(※イベント実行)
        /// </summary>
        public IObservable<Unit> PlayParticleWithEvent(Vector3 position, Quaternion rotation) {
            transform.position = position;
            transform.rotation = rotation;
            particle.Play();

            var particleEvent = GetComponent<IParticleLifeCycleEvent>();
            particleEvent?.OnPlayed();  // ※開始イベント

            // ParticleSystemのstartLifetimeに設定した秒数が経ったら終了通知
            return Observable.Timer(TimeSpan.FromSeconds(particle.main.startLifetimeMultiplier))
                .ForEachAsync(_ => {
                    particle.Stop();
                    particleEvent?.OnStopped();    // ※終了イベント
                });
        }
    }
}