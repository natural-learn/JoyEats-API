using JoyEats.IServices;
using System.Text;
using System.Text.Json;

namespace JoyEats.User.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();

            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                // 首字母小写
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                // 反序列化时忽略属性名的大小写
                PropertyNameCaseInsensitive = true,

                // 序列化时忽略值为null的属性
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        // 基础方法
        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions) ??
                throw new InvalidOperationException("HTTP GET请求未能成功反序列化响应内容。");
        }

        public async Task<T?> PostAsync<T>(string url, object data)
        {
            var jsonContent = JsonSerializer.Serialize(data, _jsonSerializerOptions);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions);
        }

        // 带查询参数的 GET 请求
        public async Task<T> GetAsync<T>(string url, object parameters)
        {
            var urlWithParams = BuildUrlWithParameters(url, parameters);
            return await GetAsync<T>(urlWithParams);
        }

        public async Task<T> GetAsync<T>(string url, IDictionary<string, object> parameters)
        {
            var urlWithParams = BuildUrlWithParameters(url, parameters);
            return await GetAsync<T>(urlWithParams);
        }

        // 带查询参数的 POST 请求
        public async Task<T?> PostAsync<T>(string url, object data, object parameters)
        {
            var urlWithParams = BuildUrlWithParameters(url, parameters);
            return await PostAsync<T>(urlWithParams, data);
        }

        // 带请求头的请求
        public async Task<T> GetAsync<T>(string url, IDictionary<string, string> headers)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            AddHeadersToRequest(request, headers);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions) ??
                throw new InvalidOperationException("HTTP GET请求未能成功反序列化响应内容。");
        }

        public async Task<T?> PostAsync<T>(string url, object data, IDictionary<string, string> headers)
        {
            var jsonContent = JsonSerializer.Serialize(data, _jsonSerializerOptions);
            using var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = httpContent
            };
            AddHeadersToRequest(request, headers);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions);
        }

        // 完整参数请求
        public async Task<T> GetAsync<T>(string url, object parameters, IDictionary<string, string> headers)
        {
            var urlWithParams = BuildUrlWithParameters(url, parameters);
            Console.WriteLine($"BuildUrlWithParameters：{urlWithParams}");
            using var request = new HttpRequestMessage(HttpMethod.Get, urlWithParams);
            AddHeadersToRequest(request, headers);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions) ??
                throw new InvalidOperationException("HTTP GET请求未能成功反序列化响应内容。");
        }

        public async Task<T?> PostAsync<T>(string url, object data, object parameters, IDictionary<string, string> headers)
        {
            var urlWithParams = BuildUrlWithParameters(url, parameters);
            var jsonContent = JsonSerializer.Serialize(data, _jsonSerializerOptions);
            using var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using var request = new HttpRequestMessage(HttpMethod.Post, urlWithParams)
            {
                Content = httpContent
            };

            AddHeadersToRequest(request, headers);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions);
        }

        // 原始数据返回
        public async Task<string> GetStringAsync(string url, object parameters = null)
        {
            var urlWithParams = BuildUrlWithParameters(url, parameters);
            return await _httpClient.GetStringAsync(urlWithParams);
        }

        public async Task<byte[]> GetByteArrayAsync(string url, object parameters = null)
        {
            var urlWithParams = BuildUrlWithParameters(url, parameters);
            return await _httpClient.GetByteArrayAsync(urlWithParams);
        }

        // 其他 HTTP 方法
        public async Task<T?> PutAsync<T>(string url, object data)
        {
            var jsonContent = JsonSerializer.Serialize(data, _jsonSerializerOptions);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions);
        }

        public async Task<T?> DeleteAsync<T>(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            // 对于 DELETE 请求，可能没有响应体
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions);
        }

        public async Task<T?> PatchAsync<T>(string url, object data)
        {
            var jsonContent = JsonSerializer.Serialize(data, _jsonSerializerOptions);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // 注意：HttpClient 默认没有 PatchAsync 方法，需要使用 SendAsync
            using var request = new HttpRequestMessage(HttpMethod.Patch, url)
            {
                Content = httpContent
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions);
        }

        private string BuildUrlWithParameters(string url, object parameters)
        {
            if (parameters == null) return url;

            var queryString = BuildQueryString(parameters);
            return string.IsNullOrEmpty(queryString) ? url : $"{url}?{queryString}";
        }

        private string BuildQueryString(object parameters)
        {
            if (parameters == null) return string.Empty;

            if (parameters is IDictionary<string, object> dictionary)
            {
                return BuildQueryStringFromDictionary(dictionary);
            }

            var properties = parameters.GetType().GetProperties();
            var keyValuePairs = properties
                .Where(p => p.CanRead && p.GetValue(parameters) != null)
                .Select(p =>
                {
                    var value = p.GetValue(parameters);
                    var stringValue = value switch
                    {
                        DateTime date => date.ToString("yyyy-MM-ddTHH:mm:ss"),
                        DateTimeOffset dateOffset => dateOffset.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        _ => value?.ToString()
                    };
                    return $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(stringValue ?? "")}";
                });
            Console.WriteLine($"{nameof(BuildQueryString)}：{string.Join("&", keyValuePairs)}");
            return string.Join("&", keyValuePairs);
        }

        private string BuildQueryStringFromDictionary(IDictionary<string, object> dictionary)
        {
            var keyValuePairs = dictionary
                .Where(kvp => kvp.Value != null)
                .Select(kvp =>
                {
                    var stringValue = kvp.Value switch
                    {
                        DateTime date => date.ToString("yyyy-MM-ddTHH:mm:ss"),
                        DateTimeOffset dateOffset => dateOffset.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        _ => kvp.Value?.ToString()
                    };
                    return $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(stringValue ?? "")}";
                });
            Console.WriteLine($"{nameof(BuildQueryStringFromDictionary)}：{string.Join("&", keyValuePairs)}");
            return string.Join("&", keyValuePairs);
        }

        private void AddHeadersToRequest(HttpRequestMessage request, IDictionary<string, string> headers)
        {
            if (headers == null) return;

            foreach (var header in headers)
            {
                if (!request.Headers.TryAddWithoutValidation(header.Key, header.Value))
                {
                    // 如果无法添加到头部，尝试添加到 Content 头部（对于有内容的请求）
                    request.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }
    }
}
