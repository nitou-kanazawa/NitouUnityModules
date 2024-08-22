using Cysharp.Threading.Tasks;

namespace nitou.SceneSystem{

    /// <summary>
    /// 各シーンに配置する起点オブジェクト
    /// </summary>
    public interface ISceneEntryPoint {

        /// <summary>
        /// シーン読み込み時の処理
        /// </summary>
        UniTask OnSceneLoadAsync();

        /// <summary>
        /// アクティブシーンに設定された時の処理
        /// </summary>
        UniTask OnSceneActivateAsync();

        /// <summary>
        /// アクティブシーンから解除された時の処理
        /// </summary>
        UniTask OnSceneDeactivateAsync();

        /// <summary>
        /// シーン解放時の処理
        /// </summary>
        UniTask OnSceneUnloadAsync();
    }
}
