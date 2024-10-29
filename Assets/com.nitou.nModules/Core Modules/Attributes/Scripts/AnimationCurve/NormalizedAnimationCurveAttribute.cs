using System;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// [参考]
//  LIGHT11: AnimationCurveを正規化するアトリビュートを作る https://light11.hatenadiary.com/entry/2021/08/11/194500

namespace nitou.Inspector {

    /// <summary>
    /// <see cref="AnimationCurve"/>の範囲を0 ~ 1に制限する属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NormalizedAnimationCurveAttribute : PropertyAttribute {

        public bool NormalizeValue { get; }
        public bool NormalizeTime { get; }

        public NormalizedAnimationCurveAttribute(bool normalizedValue = true, bool normalizedTime = true) {
            NormalizeValue = normalizedValue;
            NormalizeTime = normalizedValue;
        }

    }
}


#if UNITY_EDITOR
namespace nitou.Inspector.EditorScripts {

    [CustomPropertyDrawer(typeof(NormalizedAnimationCurveAttribute))]
    public class NormalizedAnimationCurveAttributeDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            var attr = (NormalizedAnimationCurveAttribute)attribute;

            if (property.propertyType != SerializedPropertyType.AnimationCurve) {
                // AnimationCurve以外のフィールドにアトリビュートが付けられていた場合のエラー表示
                position = EditorGUI.PrefixLabel(position, label);
                var preIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                EditorGUI.LabelField(position, "Use NormalizedAnimationCurveAttribute with AnimationCurve.");
                EditorGUI.indentLevel = preIndent;
                return;
            }

            using (var scope = new EditorGUI.ChangeCheckScope()) {
                EditorGUI.PropertyField(position, property, label, true);

                var curve = property.animationCurveValue;
                if (scope.changed) {
                    if (attr.NormalizeValue) {
                        property.animationCurveValue = property.animationCurveValue.NormalizeValue();
                    }

                    if (attr.NormalizeTime) {
                        property.animationCurveValue = property.animationCurveValue.NormalizeTime();
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property);
        }

    }
}
#endif
