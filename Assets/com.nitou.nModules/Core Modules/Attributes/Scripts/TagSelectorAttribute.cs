using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif

// [参考]
//  odin: https://www.odininspector.com/documentation/sirenix.odininspector.editor.odinattributedrawer-1
//  Using tags as a dropdown property in Unity’s inspector using PropertyDrawers https://www.brechtos.com/tagselectorattribute/

namespace nitou.Inspector {

    /// <summary>
    /// タグ選択用のドロップダウンを表示するインスペクタ属性．
    /// </summary>
   [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class TagSelectorAttribute : Attribute {
        
        public bool UseDefaultTagFieldDrawer = false;
    }

#if UNITY_EDITOR
    internal sealed class TagSelectorAttributeDrawer : OdinAttributeDrawer<TagSelectorAttribute, string> {

        protected override void DrawPropertyLayout(GUIContent label) {

            // デフォルト
            if (this.Attribute.UseDefaultTagFieldDrawer) {
                this.ValueEntry.SmartValue = EditorGUILayout.TagField(label, this.ValueEntry.SmartValue);
            } 
            
            // カスタム
            else {
                //generate the taglist + custom tags
                var tagList = new List<string>();
                tagList.Add("<NoTag>");
                tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);
                string propertyString = this.ValueEntry.SmartValue;
                int index = -1;
                if (propertyString == "") {
                    //The tag is empty
                    index = 0; //first index is the special <notag> entry
                } else {
                    //check if there is an entry that matches the entry and get the index
                    //we skip index 0 as that is a special custom case
                    for (int i = 1; i < tagList.Count; i++) {
                        if (tagList[i] == propertyString) {
                            index = i;
                            break;
                        }
                    }
                }

                //Draw the popup box with the current selected index
                index = EditorGUILayout.Popup(label.text, index, tagList.ToArray());

                //Adjust the actual string value of the property based on the selection
                this.ValueEntry.SmartValue = index switch {
                    0 => "",
                    >= 1 => tagList[index],
                    _ => ""
                };
            }

        }
    }

#endif
}
