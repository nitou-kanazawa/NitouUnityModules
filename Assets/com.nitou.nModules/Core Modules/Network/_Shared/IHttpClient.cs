using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace nitou.Networking {

    /// <summary>
    /// 
    /// </summary>
    public interface IHttpClient {

        UniTask<(HttpRequest.Result result, T response)> SendAsync<T>(HttpRequest request, Func<string, T> parseMethod,
            CancellationToken token)
            where T : HttpResponse, new();
    }


    public class HttpClient : IHttpClient {

        public async UniTask<(HttpRequest.Result result, T response)> SendAsync<T>(
           HttpRequest request, Func<string, T> parseMethod,
           CancellationToken token)
            where T : HttpResponse, new() {

            using (var unityWebRequest = UnityWebRequest.Get(request.Path)) {

                // 通信を非同期で実行
                var operation = await unityWebRequest.SendWebRequest().ToUniTask(cancellationToken: token);

                // 失敗時（通信エラー）
                if (operation.result == UnityWebRequest.Result.ConnectionError || operation.result == UnityWebRequest.Result.ProtocolError) {
                    return (new HttpRequest.Failed(), new T());
                }
                // 成功時
                else if (operation.result == UnityWebRequest.Result.Success) {
                    // レスポンスデータを取得
                    var responseData = unityWebRequest.downloadHandler.text;
                    T response;

                    // 引数として渡されたパースメソッドを利用してレスポンスデータを処理
                    try {
                        response = parseMethod(responseData);
                    } catch (Exception ex) {
                        // パース失敗時のエラーハンドリング
                        UnityEngine.Debug.LogError($"Error parsing response: {ex.Message}");
                        return (new HttpRequest.Failed(), new T());
                    }

                    return (new HttpRequest.Success(), response);
                }
                // キャンセル時
                return (new HttpRequest.Canceld(), new T());
            }
        }
    }
}
