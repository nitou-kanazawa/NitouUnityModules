
namespace nitou.ParticleModule{

    /// <summary>
    /// ParticleSystemの再生時のイベント
    /// </summary>
    public interface IParticleLifeCycleEvent {

        /// <summary>
        /// 再生開始時のイベント
        /// </summary>
        public void OnPlayed();

        /// <summary>
        /// 再生終了時のイベント
        /// </summary>
        public void OnStopped();
    }
}