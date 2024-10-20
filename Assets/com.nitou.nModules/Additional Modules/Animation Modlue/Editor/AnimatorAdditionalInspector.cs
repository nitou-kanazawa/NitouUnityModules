#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

// [参考]
//  コガネブログ: Animator の Inspector に Animator ウィンドウを開くボタンを追加するエディタ拡張 https://baba-s.hatenablog.com/entry/2022/03/18/090000

namespace nitou.Tools {
    using nitou.EditorShared;

    /// <summary>
    /// Animatorのインスペクター拡張
    /// </summary>
    [CustomEditor(typeof(Animator))]
    public sealed class AnimatorInspector : Editor {

        // オリジナルの拡張クラス
        private static readonly Type BASE_EDITOR_TYPE = typeof(Editor)
            .Assembly
            .GetType("UnityEditor.AnimatorInspector");


        /// <summary>
        /// インスペクタ描画
        /// </summary>
        public override void OnInspectorGUI() {
            var animator = (Animator)target;

            // 拡張分のインスペクター表示
            using (new EditorGUILayout.HorizontalScope()) {
                if (GUILayout.Button("Animator Window")) {
                    EditorApplication.ExecuteMenuItem("Window/Animation/Animator");
                }
                if (GUILayout.Button("Animation Window")) {
                    EditorApplication.ExecuteMenuItem("Window/Animation/Animation");
                }
            }
            EditorGUILayout.Space();

            // オリジナルのインスペクター表示
            var editor = CreateEditor(animator, BASE_EDITOR_TYPE);
            editor.OnInspectorGUI();
        }
    }
}

#endif