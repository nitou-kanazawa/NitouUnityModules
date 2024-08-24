#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;

namespace nitou.Credit.Editor {

    /// <summary>
    /// 
    /// </summary>
    public class ItemDataDrawer<TItem> : OdinValueDrawer<TItem> where TItem : CreditData {

        protected override void DrawPropertyLayout(GUIContent label) {

            var rect = EditorGUILayout.GetControlRect(label != null, 45);

            if (label != null) {
                rect.xMin = EditorGUI.PrefixLabel(rect.AlignCenterY(15), label).xMin;
            } else {
                rect = EditorGUI.IndentedRect(rect);
            }

            CreditData item = this.ValueEntry.SmartValue;
            Texture texture = null;

            if (item) {
                //texture = GUIHelper.GetAssetThumbnail(item.Icon, typeof(TItem), true);
                GUI.Label(rect.AddXMin(50).AlignMiddle(16), EditorGUI.showMixedValue ? "-" : item.englishName);
            }

            this.ValueEntry.WeakSmartValue = SirenixEditorFields.UnityPreviewObjectField(rect.AlignLeft(45), item, texture, this.ValueEntry.BaseValueType);
        }

    }

}

#endif