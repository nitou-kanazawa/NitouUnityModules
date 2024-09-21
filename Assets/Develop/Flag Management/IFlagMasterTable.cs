using UnityEngine;

namespace nitou.FlagManagement{

    public interface IFlagMasterTable {

        /// <summary>
        /// テーブルを初期化する
        /// </summary>
        void Initialize();

        /// <summary>
        /// テーブルからID検索する
        /// </summary>
        FlagEntity FindById(string id);
    }
}
