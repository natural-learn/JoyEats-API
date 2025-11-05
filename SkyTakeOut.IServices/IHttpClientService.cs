using SkyTakeOut.Core.Autofac.DependencyInjection;

namespace SkyTakeOut.IServices
{
    public interface IHttpClientService : IScopeDependency
    {
        // 基础方法
        Task<T> GetAsync<T>(string url);
        Task<T?> PostAsync<T>(string url, object data);

        // 带查询参数的 GET 请求
        Task<T> GetAsync<T>(string url, object parameters);
        Task<T> GetAsync<T>(string url, IDictionary<string, object> parameters);

        // 带查询参数的 POST 请求
        Task<T?> PostAsync<T>(string url, object data, object parameters);

        // 带请求头的请求
        Task<T> GetAsync<T>(string url, IDictionary<string, string> headers);
        Task<T?> PostAsync<T>(string url, object data, IDictionary<string, string> headers);

        // 完整参数请求
        Task<T> GetAsync<T>(string url, object parameters, IDictionary<string, string> headers);
        Task<T?> PostAsync<T>(string url, object data, object parameters, IDictionary<string, string> headers);

        // 原始数据返回
        Task<string> GetStringAsync(string url, object parameters = null);
        Task<byte[]> GetByteArrayAsync(string url, object parameters = null);

        // 其他 HTTP 方法
        Task<T?> PutAsync<T>(string url, object data);
        Task<T?> DeleteAsync<T>(string url);
        Task<T?> PatchAsync<T>(string url, object data);

    }
}
