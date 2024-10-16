using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace nitou.Networking{

    //public class TimeHttpClient {
    //    private readonly IHttpClient _httpClient;

    //    // World Time API のURL
    //    internal const string WorldTimeApiUrl = "http://worldtimeapi.org/api/timezone/Etc/UTC";

    //    public TimeHttpClient(IHttpClient httpClient) {
    //        _httpClient = httpClient;
    //    }

    //    // 現在時刻を取得するメソッド（パースメソッドを引数で渡す）
    //    public async UniTask<(HttpRequest.Result result, TimeResponse response)> GetCurrentTimeAsync(CancellationToken token, Func<string, TimeResponse> parseMethod) {
    //        var request = new TimeHttpRequest();  // 時刻リクエストを作成
    //                                              // パースメソッドを渡して現在時刻を取得
    //        var (result, response) = await timeClient.GetCurrentTimeAsync(
    //            cancellationToken,
    //            responseData => JsonUtility.FromJson<TimeResponse>(responseData)  // JsonUtility を使用してパース
    //        );

    //        var (result, response) = await _httpClient.SendAsync(request, token, parseMethod);  // パースメソッドも渡す
    //        return (result, response);
    //    }
    //}

    //// 時刻取得用のリクエストクラス
    //public class TimeHttpRequest : HttpRequest {
    //    public override string Path => TimeHttpClient.WorldTimeApiUrl;
    //}
}

