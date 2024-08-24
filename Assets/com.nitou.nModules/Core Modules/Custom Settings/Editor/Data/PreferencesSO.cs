#if UNITY_EDITOR
using UnityEditor;

// [参考]
//  hatena: エディタ拡張で「Manager」的なものに使えるScriptableSingleton https://light11.hatenadiary.com/entry/2021/03/11/201303

namespace nitou.EditorShared {

    /// <summary>
    /// エディタ用のプリファレンス設定データ
    /// </summary>
    [FilePath(
        "MyPreferences.asset", 
        FilePathAttribute.Location.PreferencesFolder
    )]
    public class PreferencesSO : ScriptableSingleton<PreferencesSO> {

        public bool flag;
        public string text;

        /// ----------------------------------------------------------------------------

        public void Save() => Save(true);
    }
}
#endif