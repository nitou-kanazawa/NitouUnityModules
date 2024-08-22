using Cysharp.Threading.Tasks;

namespace nitou.SceneSystem{

    /// <summary>
    /// 各シーンに配置する起点オブジェクト
    /// </summary>
    public interface ISceneEntryPoint {

        /// <summary>
        /// シーン読み込み時の処理
        /// </summary>
        UniTask OnSceneLoad();

        /// <summary>
        /// アクティブシーンに設定された時の処理
        /// </summary>
        UniTask OnSceneActivate();

        /// <summary>
        /// アクティブシーンから解除された時の処理
        /// </summary>
        UniTask OnSceneDeactivate();

        /// <summary>
        /// シーン解放時の処理
        /// </summary>
        UniTask OnSceneUnload();
    }
}
