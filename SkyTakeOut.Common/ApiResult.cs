using System.Text.Json.Serialization;

namespace SkyTakeOut.Common
{
    /// <summary>
    /// 统一响应模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// 响应状态码（HTTP状态码）
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 响应信息（成功/错误提示）
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 错误详情（仅在失败且开发环境下返回）
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ErrorDetail { get; set; }
    }

    /// <summary>
    /// 非泛型API响应模型（无数据返回时使用）
    /// </summary>
    public class ApiResult : ApiResult<object>
    {
        // 继承自ApiResult<object>，无需额外实现
    }
}
