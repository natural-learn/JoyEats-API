using System.ComponentModel.DataAnnotations;

namespace SkyTakeOut.Common.Configs
{
    public class MinioSettings
    {
        /// <summary>
        /// Minio服务地址
        /// </summary>
        [Required(ErrorMessage = "Minio.Endpoint不能为空")]
        public string Endpoint { get; set; }

        /// <summary>
        /// 访问密钥
        /// </summary>
        [Required(ErrorMessage = "Minio.AccessKey不能为空")]
        public string AccessKey { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        [Required(ErrorMessage = "Minio.SecretKey不能为空")]
        public string SecretKey { get; set; }

        /// <summary>
        /// 是否启用SSL（https）
        /// </summary>
        public bool IsSsl { get; set; } = false;

        /// <summary>
        /// 默认存储桶名称
        /// </summary>
        public string BucketName { get; set; }
    }
}
