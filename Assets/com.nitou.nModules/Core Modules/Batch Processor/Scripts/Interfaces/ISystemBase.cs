
namespace nitou.BachProcessor{
    
    public interface ISystemBase{
        /// <summary>
        /// システムの実行順序
        /// </summary>
        int Order { get; }
    }

    public interface IEarlyUpdate : ISystemBase {
        void OnUpdate();
    }

    public  interface IPostUpdate : ISystemBase {
        void OnLateUpdate();
    }
}