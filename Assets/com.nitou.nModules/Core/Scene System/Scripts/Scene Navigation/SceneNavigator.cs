using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

// [参考]
//  コガネブログ: シーンが読み込まれているか確認する関数 https://baba-s.hatenablog.com/entry/2022/11/28/162515
//  コガネブログ: 現在読み込まれているすべてのシーンを取得する関数 https://baba-s.hatenablog.com/entry/2022/11/28/162103
//  qiita: シーンの重複読み込みをLINQで防ぐ https://qiita.com/segur/items/b13045e6f3a9949e0503

namespace nitou.SceneSystem {

    /// <summary>
    /// EntryPoint関連の処理を追加したSceneManagerのラップクラス
    /// </summary>
    public static class SceneNavigator {

        /// <summary>
        /// 初期化が完了しているかどうか
        /// </summary>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// テスト実行（※ルートシーン以外から実行されている）かどうか
        /// </summary>
        public static bool IsTestRun { get; private set; }


        /// ----------------------------------------------------------------------------
        #region Shared Data

        // シーン間共有データ
        private static readonly Dictionary<Type, SceneSharedData> _sharedData = new();

        /// <summary>
        /// 共有データを登録する
        /// </summary>
        public static void SetData<TData>(TData data)
            where TData : SceneSharedData {

            var type = typeof(TData);
            if (_sharedData.ContainsKey(type)) {
                Debug_.LogWarning("登録されているデータを上書きします．");
            }
            _sharedData[type] = data;
        }

        /// <summary>
        /// 共有データを削除する
        /// </summary>
        public static void CleaData<TData>() 
            where TData : SceneSharedData{

            var type = typeof(TData);
            if (!_sharedData.ContainsKey(type)) {
                Debug_.LogWarning("登録されているデータはありません．");
                return;
            }

            _sharedData.Remove(type);
        }

        /// <summary>
        /// 共有データを取得する
        /// </summary>
        public static bool TryGetData<TData>(out TData data)
            where TData : SceneSharedData {

            var type = typeof(TData);
            if (!_sharedData.ContainsKey(type)) {
                Debug_.LogWarning("指定されたデータは未登録です．");
                data = null;
                return false;
            }

            data = _sharedData[type] as TData;
            return true;
        }
        #endregion


        /// ----------------------------------------------------------------------------
        // Public Methord

        /// <summary>
        /// エントリーポイントを取得する
        /// </summary>
        public static bool TryGetEntryPoint(this Scene scene, out ISceneEntryPoint entryPoint) {
            return scene.TryGetComponentInScene(out entryPoint);
        }

        /// <summary>
        /// エントリーポイントを取得する
        /// </summary>
        public static bool TryGetEntryPoint<TEntryPoint>(this Scene scene, out TEntryPoint entryPoint)
            where TEntryPoint : ISceneEntryPoint {
            return scene.TryGetComponentInScene(out entryPoint);
        }


        /// ----------------------------------------------------------------------------
        // Public Methord (汎用)

        /// <summary>
        /// シーンが読み込まれているか確認する
        /// </summary>
        public static bool IsLoaded(string sceneName) => 
            GetAllScenes().Any(x => x.name == sceneName && x.isLoaded);

        /// <summary>
        /// アクティブなシーンを取得する
        /// </summary>
        public static Scene GetActiveScene() {
            return SceneManager.GetActiveScene();
        }

        /// <summary>
        /// 現在読み込まれている全シーンを取得する
        /// </summary>
        public static IEnumerable<Scene> GetAllScenes() {
            // ※ SceneManager.GetAllScenes()は旧形式のため使用しない．

            var sceneCount = SceneManager.sceneCount;
            for (var i = 0; i < sceneCount; i++) {
                yield return SceneManager.GetSceneAt(i);
            }
        }


        /// ----------------------------------------------------------------------------
        // Public Methord

        /// <summary>
        /// シーンを読み込む．
        /// </summary>
        public static async UniTask<Scene> LoadSceneAsync(string sceneName, bool setActive = false, bool disallowSameScene = true) {

            // 既に読み込まれている場合,
            if (disallowSameScene && IsLoaded(sceneName)) {
                Debug_.LogWarning($"Scene [{sceneName}] is alredy loaded.");
                return SceneManager.GetSceneByName(sceneName);
            }

            // シーンの読み込み
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            var scene = SceneManager.GetSceneByName(sceneName);


            // アクティブなシーンに設定する
            if (setActive) {
                SceneManager.SetActiveScene(scene);
            }
            return scene;
        }

        /// <summary>
        /// シーンを取得する．存在しない場合は読み込んで取得する
        /// </summary>
        public static async UniTask<Scene> GetOrLoadSceneAsync(string sceneName, bool setActive = false) {

            // シーン読み込み
            if (!IsLoaded(sceneName)) {
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }

            var scene = SceneManager.GetSceneByName(sceneName);

            // アクティブなシーンに設定する
            if (setActive) {
                SceneManager.SetActiveScene(scene);
            }
            return scene;
        }

        /// <summary>
        /// シーンを解放する．
        /// </summary>
        public static UniTask UnLoadSceneAsync(string sceneName) {
            return SceneManager.UnloadSceneAsync(sceneName).ToUniTask();
        }

        /// <summary>
        /// シーンを解放する．
        /// </summary>
        public static UniTask UnLoadSceneAsync(Scene scene) {
            return SceneManager.UnloadSceneAsync(scene).ToUniTask();
        }


        /// ----------------------------------------------------------------------------
        // Private Methord

        internal static void RuntimeInitialize() {
            if (IsInitialized) return;

            //Debug_.Log("RuntimeInitialize");

            // 開始シーンの処理
            foreach (var scene in GetAllScenes()) {

                // 読み込みイベント
                if (scene.TryGetEntryPoint(out var entryPoint)) {
                    entryPoint.OnSceneLoadAsync();
                }
            }

            {
                // アクティブ化イベント
                var activeScene = SceneManager.GetActiveScene();
                if (activeScene.TryGetEntryPoint(out var entryPoint)) {
                    entryPoint.OnSceneActivateAsync();
                }
            }

            // --- 

            // シーン読み込み時のイベント登録
            ObservableSceneEvent.SceneLoadedAsObservable()
                .Subscribe(x => {
                    Debug_.Log($"Scene Loaded : [{x.Item1.name}] (LoadType: {x.Item2})", Colors.AliceBlue);

                    // イベント処理
                    if (x.Item1.TryGetEntryPoint(out var entry)) {
                        entry.OnSceneLoadAsync();
                    }
                });

            // シーン解放時のイベント登録
            ObservableSceneEvent.SceneUnloadedAsObservable()
                .Subscribe(x => {
                    Debug_.Log($"Scene Unloaded : [{x.name}] ", Colors.AliceBlue);

                    // イベント処理
                    if (x.TryGetEntryPoint(out var entry)) {
                        entry.OnSceneUnloadAsync();
                    }
                });

            // アクティブシーン切り替え時のイベント登録
            ObservableSceneEvent.ActiveSceneChangedAsObservable()
                .Subscribe(x => {
                    var previousScene = x.Item1;
                    var nextScene = x.Item2;

                    // イベント処理
                    ISceneEntryPoint entry;
                    if (previousScene.IsValid() && previousScene.TryGetEntryPoint(out entry)) {
                        entry.OnSceneDeactivateAsync();
                    }
                    if (nextScene.TryGetEntryPoint(out entry)) {
                        entry.OnSceneActivateAsync();
                    }

                    Debug_.Log($"Scene Changed : [{x.Item1.name}] => [{x.Item2.name}]", Colors.AliceBlue);
                });

            // フラグ更新
            IsInitialized = true;
        }
    }

}