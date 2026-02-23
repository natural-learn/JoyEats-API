using JoyEats.Common.Configs;
using JoyEats.Core.DTO;
using JoyEats.IServices;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace JoyEats.Services
{
    public class FileService : IFileService
    {
        private readonly IMinioClient _minioClient;
        private readonly MinioSettings _minioSettings;

        public FileService(IMinioClient minioClient, IOptions<MinioSettings> options)
        {
            _minioClient = minioClient;
            _minioSettings = options.Value;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="fileUploadDTO"></param>
        /// <returns></returns>
        public async Task<string> UploadFileAsync(FileUploadDTO fileUploadDTO)
        {
            using (fileUploadDTO)
            {
                var bucketName = _minioSettings.BucketName;
                var objectName = Guid.NewGuid().ToString() + Path.GetExtension(fileUploadDTO.FileName);
                string dateDir = DateTime.Now.ToString("yyyy-MM-dd");
                string fullName = $"{dateDir}/{objectName}";
                if (_minioClient == null)
                {
                    throw new Exception("MinIO客户端未初始化");
                }
                // 检查存储桶是否存在，不存在则创建
                var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
                if (!bucketExists)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
                }

                // 上传文件
                await _minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fullName)
                    .WithStreamData(fileUploadDTO.ContentStream)
                    .WithObjectSize(fileUploadDTO.ContentStream.Length)
                    .WithContentType(fileUploadDTO.ContentType));

                return $"http://{_minioSettings.Endpoint}/{_minioSettings.BucketName}/{fullName}";
            }
        }
    }
}
