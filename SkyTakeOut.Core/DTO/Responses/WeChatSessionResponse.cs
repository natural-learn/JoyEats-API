using System.Text.Json.Serialization;

namespace SkyTakeOut.Core.DTO.Responses
{
    /// <summary>
    /// 微信会话响应（获取执行微信登录后返回的数据）
    /// </summary>
    public class WeChatSessionResponse
    {
        [JsonPropertyName("openid")]
        public string OpenId { get; set; }

        [JsonPropertyName("session_key")]
        public string SessionKey { get; set; }

        [JsonPropertyName("errcode")]
        public int ErrCode { get; set; } // 错误码，0表示成功

        [JsonPropertyName("errmsg")]
        public string ErrMsg { get; set; } // 错误信息

        /// <summary>
        /// 判断请求是否成功
        /// </summary>
        public bool IsSuccess => ErrCode == 0;
    }
}
