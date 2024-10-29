using UnityEngine;

// [参考]
//  PG日誌: OdinのColorフィールド拡張を無効化する https://takap-tech.com/entry/2023/08/11/005353

namespace nitou {

    /// <summary>
    /// Odinの拡張表示を無効化する属性（※Colorフィールドで値コピーができなくなるのを防ぐために使用する）
    /// </summary>
    public class OdinIgnoreAttribute : PropertyAttribute{}
}
