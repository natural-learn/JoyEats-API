using JoyEats.Common;
using JoyEats.Common.Constant;
using JoyEats.Core.DTO;
using JoyEats.IServices;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.Admin.Controllers
{
    /// <summary>
    /// 通用接口
    /// </summary>
    [Route("admin/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ILogger<CommonController> _logger;
        private readonly IFileService _fileService;

        public CommonController(ILogger<CommonController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ApiResult<string>>> Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return ApiResultHelper.Error<string>("文件为空");
                }
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                var fileUploadDTO = new FileUploadDTO
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    ContentStream = memoryStream
                };
                string url = await _fileService.UploadFileAsync(fileUploadDTO);
                return ApiResultHelper.Success<string>(url);
            }
            catch (Exception ex)
            {
                _logger.LogError("文件上传失败：{@string}", ex.Message);
            }
            return ApiResultHelper.Error<string>(MessageConstant.UPLOAD_FAILED);
        }
    }
}
