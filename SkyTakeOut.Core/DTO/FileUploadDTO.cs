namespace SkyTakeOut.Core.DTO
{
    public class FileUploadDTO : IDisposable
    {
        /// <summary>
        /// 文件名（含扩展名）
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件MIME类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 文件内容流
        /// </summary>
        public Stream ContentStream { get; set; }

        /// <summary>
        /// 确保流被释放（避免资源泄漏）
        /// </summary>
        public void Dispose()
        {
            ContentStream?.Dispose();
        }
    }
}
