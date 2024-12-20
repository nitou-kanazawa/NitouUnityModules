#if UNITY_EDITOR
using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace nitou.LevelActors.Inputs.EditorScripts {
    using nitou.EditorShared;

    [CustomEditor(typeof(ActorActionsAsset))]
    internal class LevelActorActionsAssetEditor : Editor {

        // 
        const string CharacterActionsFileName = "CharacterActions";
        const string TemplateFileName = "Template-character-actions";

        SerializedProperty boolActions = null;
        SerializedProperty floatActions = null;
        SerializedProperty vector2Actions = null;


        /// ----------------------------------------------------------------------------
        // Editor Method

        private void OnEnable() {
            boolActions = serializedObject.FindProperty("boolActions");
            floatActions = serializedObject.FindProperty("floatActions");
            vector2Actions = serializedObject.FindProperty("vector2Actions");
        }

        public override void OnInspectorGUI() {
            EditorUtil.GUI.ScriptableObjectField((ActorActionsAsset)target);

            serializedObject.Update();

            EditorUtil.GUI.HorizontalLine();
            {
                EditorGUILayout.PropertyField(boolActions, true);
                EditorGUILayout.PropertyField(floatActions, true);
                EditorGUILayout.PropertyField(vector2Actions, true);
            }
            EditorUtil.GUI.HorizontalLine();

            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(
                "Click the button to replace the original \"CharacterActions.cs\" file. This can be useful if you need to create custom actions, without modifing the code. ",
                MessageType.Info
            );

            // コード出力ボタン
            if (GUILayout.Button("Create scripte")) {

                // 確認モーダル
                bool isYes = EditorUtility.DisplayDialog(
                    "Create actions",
                    "Warning: This will replace the original \"CharacterActions\" file. Are you sure you want to continue?",
                    "Yes", "No");

                if (isYes) {

                    // テンプレート
                    string templatePath = AssetDatabase.FindAssets(TemplateFileName)
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .FirstOrDefault();
                    if (templatePath.IsNullOrEmpty()) {
                        Debug_.LogWarning("Template file cannot be found.");
                        return;
                    }

                    // 対象スクリプト
                    var targetFilePath = AssetDatabase.FindAssets(CharacterActionsFileName + " t:script")
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .FirstOrDefault(str => string.Equals(Path.GetFileNameWithoutExtension(str), CharacterActionsFileName, StringComparison.CurrentCultureIgnoreCase));

                    // 対象スクリプトのディレクトリ
                    var DirectoryPath = !targetFilePath.IsNullOrEmpty()
                        ? Path.GetDirectoryName(targetFilePath)
                        : "Assets";     // ※存在していない場合，Assets直下に新規生成

                    // ファイル出力
                    var filePath = EditorUtility.SaveFilePanel("Save", DirectoryPath, CharacterActionsFileName, "cs");
                    if (!string.IsNullOrEmpty(filePath)) {
                        Debug_.Log(filePath, Colors.Orange);
                        CreateCSharpClass(filePath, templatePath);
                    }
                }

            }
            serializedObject.ApplyModifiedProperties();
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private void CreateCSharpClass(string characterActionsPath, string templatePath) {
            if (characterActionsPath == null || templatePath == null) return;

            string output = GenerateOutput(CharacterActionsFileName, templatePath);

            FileStream fileStream = File.Open(characterActionsPath, FileMode.Truncate, FileAccess.ReadWrite);

            StreamWriter file = new StreamWriter(fileStream);

            file.Write(output);
            file.Close();

            AssetDatabase.Refresh();
        }

        private string GenerateOutput(string className, string templatePath) {
            StreamReader reader = new StreamReader(templatePath);

            string output = reader.ReadToEnd();
            reader.Close();

            output = Regex.Replace(output, @"@\s*struct-name\s*@", CharacterActionsFileName);

            // -----------------------------------------------------------------------------------------------------------------------------------
            // Bool Actions ----------------------------------------------------------------------------------------------------------------------
            // -----------------------------------------------------------------------------------------------------------------------------------

            string definitionsString = "";
            string resetString = "";
            string newString = "";
            string setValueString = "";
            string copyValueString = "";
            string updateString = "";

            for (int i = 0; i < boolActions.arraySize; i++) {
                string actionName = boolActions.GetArrayElementAtIndex(i).stringValue;
                if (actionName.IsNullOrEmpty())
                    continue;

                string[] words = actionName.Split(' ');

                string variableName = "@";
                for (int j = 0; j < words.Length; j++) {
                    string word = words[j];

                    if (j == 0)
                        variableName += System.Char.ToLowerInvariant(word[0]) + word.Substring(1).ToLower();
                    else
                        variableName += System.Char.ToUpperInvariant(word[0]) + word.Substring(1).ToLower();
                }


                definitionsString += "\tpublic BoolAction " + variableName + ";\n";
                resetString += "\t\t" + variableName + ".Reset();\n";
                newString += "\t\t" + variableName + " = new BoolAction();\n" +
                    "\t\t" + variableName + ".Initialize();\n\n";
                setValueString += "\t\t" + variableName + ".value = inputHandler.GetBool( \"" + actionName + "\" );\n";
                copyValueString += "\t\t" + variableName + ".value = characterActions." + variableName.Substring(1) + ".value;\n";
                updateString += "\t\t" + variableName + ".Update( dt );\n";
            }

            // Write bool actions
            output = Regex.Replace(output, @"@\s*bool-actions-definitions\s*@", definitionsString);
            output = Regex.Replace(output, @"@\s*bool-actions-reset\s*@", resetString);
            output = Regex.Replace(output, @"@\s*bool-actions-new\s*@", newString);
            output = Regex.Replace(output, @"@\s*bool-actions-setValue\s*@", setValueString);
            output = Regex.Replace(output, @"@\s*bool-actions-copyValue\s*@", copyValueString);
            output = Regex.Replace(output, @"@\s*bool-actions-update\s*@", updateString);

            // -----------------------------------------------------------------------------------------------------------------------------------
            // Float Actions ---------------------------------------------------------------------------------------------------------------------
            // -----------------------------------------------------------------------------------------------------------------------------------

            definitionsString = "";
            resetString = "";
            newString = "";
            setValueString = "";
            copyValueString = "";
            updateString = "";

            for (int i = 0; i < floatActions.arraySize; i++) {
                string actionName = floatActions.GetArrayElementAtIndex(i).stringValue;
                if (actionName.IsNullOrEmpty())
                    continue;

                string[] words = actionName.Split(' ');

                string variableName = "@";
                for (int j = 0; j < words.Length; j++) {
                    string word = words[j];

                    if (j == 0)
                        variableName += System.Char.ToLowerInvariant(word[0]) + word.Substring(1).ToLower();
                    else
                        variableName += System.Char.ToUpperInvariant(word[0]) + word.Substring(1).ToLower();
                }


                definitionsString += "\tpublic FloatAction " + variableName + ";\n";
                resetString += "\t\t" + variableName + ".Reset();\n";
                newString += "\t\t" + variableName + " = new FloatAction();\n";
                setValueString += "\t\t" + variableName + ".value = inputHandler.GetFloat( \"" + actionName + "\" );\n";
                copyValueString += "\t\t" + variableName + ".value = characterActions." + variableName.Substring(1) + ".value;\n";
            }

            // Write bool actions
            output = Regex.Replace(output, @"@\s*float-actions-definitions\s*@", definitionsString);
            output = Regex.Replace(output, @"@\s*float-actions-reset\s*@", resetString);
            output = Regex.Replace(output, @"@\s*float-actions-new\s*@", newString);
            output = Regex.Replace(output, @"@\s*float-actions-setValue\s*@", setValueString);
            output = Regex.Replace(output, @"@\s*float-actions-copyValue\s*@", copyValueString);

            // -----------------------------------------------------------------------------------------------------------------------------------
            // Vector2 Actions -------------------------------------------------------------------------------------------------------------------
            // -----------------------------------------------------------------------------------------------------------------------------------

            definitionsString = "";
            resetString = "";
            newString = "";
            setValueString = "";
            updateString = "";

            for (int i = 0; i < vector2Actions.arraySize; i++) {
                string actionName = vector2Actions.GetArrayElementAtIndex(i).stringValue;
                if (actionName.IsNullOrEmpty())
                    continue;

                string[] words = actionName.Split(' ');

                string variableName = "@";
                for (int j = 0; j < words.Length; j++) {
                    string word = words[j];

                    if (j == 0)
                        variableName += System.Char.ToLowerInvariant(word[0]) + word.Substring(1).ToLower();
                    else
                        variableName += System.Char.ToUpperInvariant(word[0]) + word.Substring(1).ToLower();
                }


                definitionsString += "\tpublic Vector2Action " + variableName + ";\n";
                resetString += "\t\t" + variableName + ".Reset();\n";
                newString += "\t\t" + variableName + " = new Vector2Action();\n";
                setValueString += "\t\t" + variableName + ".value = inputHandler.GetVector2( \"" + actionName + "\" );\n";
                copyValueString += "\t\t" + variableName + ".value = characterActions." + variableName.Substring(1) + ".value;\n";

            }

            // Write bool actions
            output = Regex.Replace(output, @"@\s*vector2-actions-definitions\s*@", definitionsString);
            output = Regex.Replace(output, @"@\s*vector2-actions-reset\s*@", resetString);
            output = Regex.Replace(output, @"@\s*vector2-actions-new\s*@", newString);
            output = Regex.Replace(output, @"@\s*vector2-actions-setValue\s*@", setValueString);
            output = Regex.Replace(output, @"@\s*vector2-actions-copyValue\s*@", copyValueString);

            return output;

        }
    }

}

#endif
