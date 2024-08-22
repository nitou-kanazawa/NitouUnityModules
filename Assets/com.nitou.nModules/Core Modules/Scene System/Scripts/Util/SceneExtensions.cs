using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nitou {

    /// <summary>
    /// <see cref="Scene"/>を対象とした拡張メソッド集
    /// </summary>
    public static class SceneExtensions {

        /// ----------------------------------------------------------------------------
        // Public Methord

        /// <summary>
        /// 指定シーン内のルートからコンポーネントを取得する
        /// </summary>
        public static bool TryGetComponentInSceneRoot<T>(this Scene scene, out T result) {

            if (!scene.IsValid()) {
                throw new ArgumentException("Scene is invalid.", nameof(scene));
            }

            // シーン内のルートオブジェクトを順にチェックする
            foreach (GameObject rootObj in scene.GetRootGameObjects()) {
                if (rootObj.TryGetComponent(out result)) {
                    return true;
                }
            }

            // ※見つからなかった場合，
            result = default;
            return false;
        }

        /// <summary>
        /// 指定シーン内のコンポーネントを取得する
        /// </summary>
        public static bool TryGetComponentInScene<T>(this Scene scene, out T result, bool includeInactive = true) {

            if (!scene.IsValid()) {
                throw new ArgumentException("Scene is invalid.", nameof(scene));
            }

            // シーン内のルートオブジェクトを順にチェックする
            foreach (GameObject rootObj in scene.GetRootGameObjects()) {
                result = rootObj.GetComponentInChildren<T>(includeInactive);
                if (result != null) {
                    return true;
                }
            }

            // ※見つからなかった場合，
            result = default;
            return false;
        }

        /// <summary>
        /// 指定シーン内のコンポーネントを取得する
        /// </summary>
        public static T GetComponentInScene<T>(this Scene scene, bool includeInactive = true) {
            return TryGetComponentInScene(scene, out T result, includeInactive)
                ? result
                : throw new InvalidOperationException($"Component of type '{typeof(T).Name}' is not found in scene '{scene.name}'.");
        }

        /// <summary>
        /// 指定シーン内のコンポーネントを取得する
        /// </summary>
        public static T GetComponentInSceneOrDefault<T>(this Scene scene, bool includeInactive = true) {
            TryGetComponentInScene(scene, out T result, includeInactive);
            return result;
        }

    }
}
