using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Core.DTO;

namespace SkyTakeOut.IServices
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
