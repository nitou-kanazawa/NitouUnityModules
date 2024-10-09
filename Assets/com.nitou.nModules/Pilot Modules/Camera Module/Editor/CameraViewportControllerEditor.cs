#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace nitou.CameraModule.EditorSctipts {
    using nitou.EditorShared;

    /// <summary>
    /// <see cref="CameraViewportController"/>のインスペクタ拡張
    /// </summary>
    //[CustomEditor(typeof(CameraViewportController))]
    public class CameraViewportControllerEditor : Editor {

        public override void OnInspectorGUI() {

            var instance = target as CameraViewportController;

            // コンポーネント説明文
            var message = "カメラのViewport値を制御するコンポーネント.\n"
                + "指定したUI要素のRectにViewportをフィットさせます.";
            EditorGUILayout.HelpBox(message, MessageType.Info);

            // 
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            //using (new EditorGUI.IndentLevelScope()) {
            //    var rectTrans = EditorGUILayout.ObjectField("Target Rect", instance._targetRect, typeof(RectTransform), true) as RectTransform;
            //    instance.SetTarget(rectTrans);
            //    instance.apply = EditorGUILayout.Toggle("Apply", instance.apply);
            //    if (instance.apply) {
            //        // ※smoothTimeは正の値に制限する
            //        var newValue = EditorGUILayout.FloatField("SmoothTime", instance.smoothTime);
            //        instance.smoothTime = Mathf.Max(0f, newValue);
            //    }
            //}
        }
    }
}
#endif