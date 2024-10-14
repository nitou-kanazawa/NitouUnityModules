
namespace nitou.Networking {

    /// <summary>
    /// リクエストの基底クラス
    /// </summary>
    public abstract class HttpRequest {
        
        public abstract string Path { get; }


        /// ----------------------------------------------------------------------------
        #region Result

        /// <summary>
        /// 通信自体が正常に行われたかの結果
        /// </summary>
        public abstract record Result() {
            public bool IsSuccess() => this is Success;
            public bool IsCanceled() => this is Canceld;
            public bool IsFiled() => this is Failed;
        }

        public record Success() : Result;
        public record Canceld() : Result;
        public record Failed() : Result;
        #endregion
    }
}
