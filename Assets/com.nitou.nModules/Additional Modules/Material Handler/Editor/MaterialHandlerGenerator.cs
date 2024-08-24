#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace nitou.MaterialControl.EditorScript {
    using nitou.EditorShared;

    /// <summary>
    /// <see cref="MaterialHandler"/>の継承クラスを自動生成するエディタウインドウ
    /// </summary>
    public class MaterialHandlerGenerator : EditorWindow {

        private Shader _shader;
        private string generatedClassText;

        // 内部処理用
        private Vector2 _scrollPosition;


        /// ----------------------------------------------------------------------------
        // EditorWindow Method 

        [MenuItem(MenuItemName.Prefix.EditorWindow + "Material/Material Handler Generator")]
        public static void ShowWindow() {
            GetWindow<MaterialHandlerGenerator>("Material Handler Generator");
        }

        private void OnGUI() {
            GUILayout.Label("Generate Material Handler Class", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope()) {

                _shader = EditorGUILayout.ObjectField("Shader", _shader, typeof(Shader), false) as Shader;

                if (_shader != null) {
                    EditorGUILayout.Space();
                    using (new EditorGUILayout.HorizontalScope()) {

                        // PREVIEWボタン
                        using (new EditorUtil.GUIBackgroundColorScope(Colors.GreenYellow)) {
                            if (GUILayout.Button("Generate Preview")) {
                                generatedClassText = GenerateMaterialHandlerClassText(_shader);
                            }
                        }
                        // CLEARボタン
                        using (new EditorGUI.DisabledScope(generatedClassText.IsNullOrEmpty())) {
                            if (GUILayout.Button("Clear")) {
                                generatedClassText = string.Empty;
                            }
                        }
                    }
                }

                // Previewテキスト表示
                if (!generatedClassText.IsNullOrEmpty()) {
                    EditorGUILayout.Space();
                    GUILayout.Label("Generated Class Preview", EditorStyles.boldLabel);

                    // テキスト表示をスクロール可能に
                    using (var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPosition, GUILayout.Height(300))) {
                        _scrollPosition = scrollView.scrollPosition;
                        GUILayout.TextArea(generatedClassText, EditorUtil.Styles.textArea, GUILayout.ExpandHeight(true));
                    }

                    // ファイル出力
                    EditorGUILayout.Space();
                    if (GUILayout.Button("Save Class")) {
                        SaveGeneratedClass(_shader, generatedClassText);
                    }
                }
            }
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        /// <summary>
        /// 
        /// </summary>
        private string GenerateMaterialHandlerClassText(Shader shader) {

            string className = shader.name.Replace("/", "_") + "Material";
            var writer = new StringWriter();

            // 
            writer.WriteLine("using UnityEngine;");
            writer.WriteLine("namespace nitou.MaterialControl");
            writer.WriteLine("{");
            writer.WriteLine($"    public class {className} : {typeof(MaterialHandler).Name}");
            writer.WriteLine("    {");

            // Fieldの書き出し
            int propertyCount = ShaderUtil.GetPropertyCount(shader);
            for (int i = 0; i < propertyCount; i++) {
                string propertyName = ShaderUtil.GetPropertyName(shader, i);
                string propertyID = $"_{propertyName}ID";
                writer.WriteLine($"        protected int {propertyID} = Shader.PropertyToID(\"{propertyName}\");");
            }

            writer.WriteLine("");

            // 次にプロパティを書き出す
            for (int i = 0; i < propertyCount; i++) {
                string propertyName = ShaderUtil.GetPropertyName(shader, i);
                string cleanPropertyName = propertyName.TrimStart('_');
                ShaderUtil.ShaderPropertyType propertyType = ShaderUtil.GetPropertyType(shader, i);
                string propertyID = $"_{propertyName}ID";

                switch (propertyType) {
                    case ShaderUtil.ShaderPropertyType.Color:
                        writer.WriteLine($"        public Color {cleanPropertyName}");
                        writer.WriteLine("        {");
                        writer.WriteLine($"            get => _material.GetColor({propertyID});");
                        writer.WriteLine($"            set => _material.SetColor({propertyID}, value);");
                        writer.WriteLine("        }");
                        break;
                    case ShaderUtil.ShaderPropertyType.Vector:
                        writer.WriteLine($"        public Vector4 {cleanPropertyName}");
                        writer.WriteLine("        {");
                        writer.WriteLine($"            get => _material.GetVector({propertyID});");
                        writer.WriteLine($"            set => _material.SetVector({propertyID}, value);");
                        writer.WriteLine("        }");
                        break;
                    case ShaderUtil.ShaderPropertyType.Float:
                    case ShaderUtil.ShaderPropertyType.Range:
                        writer.WriteLine($"        public float {cleanPropertyName}");
                        writer.WriteLine("        {");
                        writer.WriteLine($"            get => _material.GetFloat({propertyID});");
                        writer.WriteLine($"            set => _material.SetFloat({propertyID}, value);");
                        writer.WriteLine("        }");
                        break;
                    case ShaderUtil.ShaderPropertyType.TexEnv:
                        writer.WriteLine($"        public Texture {cleanPropertyName}");
                        writer.WriteLine("        {");
                        writer.WriteLine($"            get => _material.GetTexture({propertyID});");
                        writer.WriteLine($"            set => _material.SetTexture({propertyID}, value);");
                        writer.WriteLine("        }");
                        break;
                }
            }

            writer.WriteLine("");
            writer.WriteLine($"        public {className}(Shader shader) : base(shader) {{ }}");

            writer.WriteLine("    }");
            writer.WriteLine("}");

            return writer.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveGeneratedClass(Shader shader, string classText) {
            /*
            string assetPath = this.GetAssetPath();
            string parentDirectory = Directory.GetParent(assetPath).FullName;
            string className = shader.name.Replace("/", "_") + "Material";
            string filePath = Path.Combine(parentDirectory, className + ".cs");

            if (EditorUtility.DisplayDialog("Save Generated Class", $"Save generated class to {filePath}?", "Save", "Cancel")) {
                File.WriteAllText(filePath, classText);
                AssetDatabase.Refresh();
                Debug.Log($"Material handler class {className} generated at {filePath}");
            }
            */

            string defaultName = shader.name.Replace("/", "_") + "Material.cs";
            string path = EditorUtility.SaveFilePanel("Save Generated Class", "", defaultName, "cs");

            if (!string.IsNullOrEmpty(path)) {
                File.WriteAllText(path, classText);
                AssetDatabase.Refresh();
                Debug.Log($"Material handler class {defaultName} generated at {path}");
            }

        }

       
    }
}
#endif