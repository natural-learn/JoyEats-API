using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Core.DTO;

namespace JoyEats.IServices
{
    public interface IFileService : IScopeDependency
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="fileUploadDTO"></param>
        /// <returns></returns>
        public Task<string> UploadFileAsync(FileUploadDTO fileUploadDTO);
    }
}
