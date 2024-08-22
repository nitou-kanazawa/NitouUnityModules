using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UniRx;

// [参考]
//  qiita: シーンの読み込みイベントをIObservableにする https://qiita.com/su10/items/93977e0b95449ec1b944
//  qiita: UniRxのObservable.FromEventを使う https://qiita.com/ShirakawaMaru/items/4071aad0937ecbdc7fe9

namespace nitou.SceneSystem {

    /// <summary>
    /// <see cref="SceneManager"/>のイベントをObserbableに変換するライブラリ
    /// </summary>
    public static class ObservableSceneEvent {

        /// <summary>
        /// "activeSceneChanged"イベントをObservableに変換する
        /// </summary>
        public static IObservable<Tuple<Scene, Scene>> ActiveSceneChangedAsObservable() {
            return Observable.FromEvent<UnityAction<Scene, Scene>, Tuple<Scene, Scene>>(
                h => (x, y) => h(Tuple.Create(x, y)),
                h => SceneManager.activeSceneChanged += h,
                h => SceneManager.activeSceneChanged -= h
                );
        }

        /// <summary>
        /// "sceneLoaded"イベントをObservableに変換する
        /// </summary>
        public static IObservable<Tuple<Scene, LoadSceneMode>> SceneLoadedAsObservable() {
            return Observable.FromEvent<UnityAction<Scene, LoadSceneMode>, Tuple<Scene, LoadSceneMode>>(
                h => (x, y) => h(Tuple.Create(x, y)),
                h => SceneManager.sceneLoaded += h,
                h => SceneManager.sceneLoaded -= h
            );
        }

        /// <summary>
        /// "sceneUnloaded"イベントをObservableに変換する
        /// </summary>
        public static IObservable<Scene> SceneUnloadedAsObservable() {
            return Observable.FromEvent<UnityAction<Scene>, Scene>(
                h => h.Invoke,
                h => SceneManager.sceneUnloaded += h,
                h => SceneManager.sceneUnloaded -= h
            );
        }
    }
}
