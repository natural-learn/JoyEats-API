using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Core.DTO.Category;
using SkyTakeOut.IServices;

namespace SkyTakeOut.Admin.Controllers
{
    [Route("admin/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        public async Task<ActionResult<ApiResult>> Save([FromBody] CategoryDTO categoryDTO)
        {
            _logger.LogInformation("新增分类：{@CategoryRequest}", categoryDTO);
            await _categoryService.SaveAsync(categoryDTO);
            return ApiResultHelper.Success();
        }
    }
}
