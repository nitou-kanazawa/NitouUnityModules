using System.Collections.Generic;
using UnityEngine;

// [参考]
//  くろくまそふと: フラグ管理機能の作り方！ゲーム開発に必須の機能を自作してみよう https://kurokumasoft.com/2022/04/28/unity-flag-management/

namespace nitou.FlagManagement {

    public sealed class FlagMasterTable {

        private Dictionary<string, bool> _flags = new Dictionary<string, bool>(){
            { "NewFeatureA", false },
            { "ExperimentalFeatureB", true }
        };

    }
}
