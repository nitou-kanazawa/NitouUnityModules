#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Text;

// [参考]
//  youtube: Custom Editor Windows Made Easy with Odin Inspector https://www.youtube.com/watch?v=erWEG-6hx7g

namespace nitou.Credit.Editor {
    using nitou.EditorShared;

    public class CreditInfoEditorWindow : OdinMenuEditorWindow {

        /// ----------------------------------------------------------------------------
        // Field & Properity

        private CreateNewCreditData _createNewCreditData;
        private CreditTextCreater _creditTextCreater;

        private string RootFolderPath => PathUtil.Combine(this.GetAssetParentFolderPath(2), "Scriptable Objects");


        /// ----------------------------------------------------------------------------
        // Editor Method

        [MenuItem( ToolBarMenu.Prefix.EditorWindow + "Credit Info")]
        private static void OpenWindow() => GetWindow<CreditInfoEditorWindow>().Show();

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_createNewCreditData != null) {
                DestroyImmediate(_createNewCreditData.data);
            }
        }

        /// <summary>
        /// サイドメニューの構成
        /// </summary>
        protected override OdinMenuTree BuildMenuTree() {

            var tree = new OdinMenuTree();

            // 新規データの作成
            _createNewCreditData = new CreateNewCreditData(this);
            tree.Add("Add New Data", _createNewCreditData, SdfIconType.PlusSquareFill);

            // データ一覧の表示
            tree.AddAllAssetsAtPath("Credit Data", RootFolderPath, typeof(CreditData), includeSubDirectories: true);

            //
            _creditTextCreater = new CreditTextCreater(this);
            tree.Add("Convert to Text", _creditTextCreater, SdfIconType.ChatSquareTextFill);

            return tree;
        }

        /// <summary>
        /// GUI描画の定義
        /// </summary>
        protected override void OnBeginDrawEditors() {
            if (MenuTree == null) return;
            OdinMenuTreeSelection selected = this.MenuTree.Selection;

            // ツールバーの描画
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                // 削除ボタン
                if (SirenixEditorGUI.ToolbarButton("Delete Current")) {
                    CreditData asset = selected.SelectedValue as CreditData;
                    string path = AssetDatabase.GetAssetPath(asset);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        /// <summary>
        /// クレジットの種類ごとの親フォルダ名を取得する
        /// </summary>
        private static string GetCreditDataFolderName(CreditType type) =>
            type switch {
                CreditType.Font => "Font Credit",
                CreditType.Image => "Image Credit",
                CreditType.Model => "Model Credit",
                CreditType.Sound => "Sound Credit",
                CreditType.Script => "Script Credit",
                _ => throw new NotImplementedException(),
            };


        // ----------------------------------------------------------------------------
        #region TreeMenue    

        /// <summary>
        /// 新規データを作成するメニュー
        /// </summary>
        public class CreateNewCreditData {

            /// ----------------------------------------------------------------------------
            // Field & Properity

            private readonly CreditInfoEditorWindow context;

            // クレジットの種類            
            [OnValueChanged("CreateInstance")]
            [BoxGroup] public CreditType creditType;

            [Space(20)]

            [InlineEditor(objectFieldMode: InlineEditorObjectFieldModes.Hidden)]
            public CreditData data;


            /// ----------------------------------------------------------------------------
            // Public Method

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CreateNewCreditData(CreditInfoEditorWindow context) {
                this.context = context;
                CreateInstance();
            }


            /// ----------------------------------------------------------------------------
            // Private Method

            /// <summary>
            /// 指定した種類のクレジットデータを生成する
            /// </summary>
            private void CreateInstance() {
                //Debug.Log("Create data");

                var classType = creditType.GetClassType();
                var newData = ScriptableObject.CreateInstance(classType) as CreditData;

                if (data != null) {
                    newData.englishName = data.englishName;
                    newData.url = data.url;
                    newData.isUsed = data.isUsed;
                    newData.description = data.description;
                }

                data = newData;
            }

            /// <summary>
            /// アセットとして新規データを保存する
            /// </summary>
            [Button("Add New Credit Data")]
            private void SaveNewData() {
                var assetName = $"{data.Type}_{data.englishName}.asset";
                var assetPath = PathUtil.Combine(
                    context.RootFolderPath,
                    GetCreditDataFolderName(creditType),
                    assetName
                    );

                if (Directory.Exists(assetPath)) {
                    Debug_.LogWarning($"指定したファイル名{assetName}は，既に存在します.");
                    return;
                }

                AssetDatabase.CreateAsset(data, assetPath);
                AssetDatabase.SaveAssets();
                data = null;
                CreateInstance();
            }

        }


        /// <summary>
        /// クレジットデータから表示テキストを作成するメニュー
        /// </summary>
        public class CreditTextCreater {

            /// ----------------------------------------------------------------------------
            // Field & Properity

            private readonly CreditInfoEditorWindow context;

            public bool useFontCredit;
            public bool useImageCredit;
            public bool useModelCredit;
            public bool useSoundCredit;
            public bool useScriptCredit;

            [TextArea(5, 20), HideLabel]
            public string message;


            /// ----------------------------------------------------------------------------
            // Public Method

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CreditTextCreater(CreditInfoEditorWindow context) {
                this.context = context;
            }

            /// <summary>
            /// 
            /// </summary>
            [Button]
            public void UpdateText() {

                var sb = new StringBuilder();

                if (useFontCredit) {
                    sb.Append("========================\n");
                    sb.Append(" Font \n");
                    sb.Append("========================\n\n");

                    var datalist = LoadCreditData(CreditType.Font);
                    sb.Append(datalist.Convert());
                }

                if (useImageCredit) {
                    sb.Append("========================\n");
                    sb.Append(" Image \n");
                    sb.Append("========================\n\n");

                    var datalist = LoadCreditData(CreditType.Image);
                    sb.Append(datalist.Convert());
                }


                if (useSoundCredit) {
                    sb.Append("========================\n");
                    sb.Append(" Sound \n");
                    sb.Append("========================\n\n");

                    var datalist = LoadCreditData(CreditType.Sound);
                    sb.Append(datalist.Convert());
                }

                if (useScriptCredit) {
                    sb.Append("========================\n");
                    sb.Append(" Script \n");
                    sb.Append("========================\n\n");

                    var datalist = LoadCreditData(CreditType.Script);
                    sb.Append(datalist.Convert());
                }


                // 
                message = sb.ToString();
            }

            /// ----------------------------------------------------------------------------
            // Private Method

            /// <summary>
            /// 指定した種類のクレジットデータを読み込む
            /// </summary>
            private IReadOnlyList<CreditData> LoadCreditData(CreditType type) {

                var folderPath = PathUtil.Combine(
                    context.RootFolderPath,
                    GetCreditDataFolderName(type)
                    );

                var assets = NonResources.LoadAll<CreditData>(folderPath)
                    .Where(x => x.isUsed)
                    .OrderBy(x => x.englishName)
                    .ToList();
                return assets;
            }



        }

        #endregion

    }
}

#endif