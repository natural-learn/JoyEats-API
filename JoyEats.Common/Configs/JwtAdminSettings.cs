namespace JoyEats.Common.Configs
{
    public class JwtAdminSettings
    {
        /// <summary>
        /// 用于签名和验证Token的密钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Token的签发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 接收方
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 过期时间（分钟）
        /// </summary>
        public int ExpireMinutes { get; set; }
    }
}
