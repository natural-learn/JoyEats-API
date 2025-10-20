using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Core.DTO.Category;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;

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
        [HttpPost]
        public async Task<ActionResult<ApiResult>> Save([FromBody] CategoryDTO categoryDTO)
        {
            _logger.LogInformation("新增分类：{@CategoryRequest}", categoryDTO);
            await _categoryService.SaveAsync(categoryDTO);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 分类分页查询
        /// </summary>
        /// <param name="categoryPageQueryDTO"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<ActionResult<ApiResult<PagedResult<Category>>>> Page([FromQuery] CategoryPageQueryDTO categoryPageQueryDTO)
        {
            _logger.LogInformation("分类分页查询：{@CategoryPageQueryDTO}", categoryPageQueryDTO);
            var pagedResult = await _categoryService.PageQueryAsync(categoryPageQueryDTO);
            return ApiResultHelper.Success(pagedResult);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<ApiResult>> DeleteById(long id)
        {
            _logger.LogInformation("删除分类：{@long}", id);
            if (id <= 0)
            {
                return ApiResultHelper.Error("分类id必须为正整数");
            }
            await _categoryService.DeleteByCategoryIdAsync(id);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResult>> Update([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO.Id <= 0)
            {
                return ApiResultHelper.Error("分类id必须为正整数");
            }
            await _categoryService.UpdateCategoryAsync(categoryDTO);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 启用、禁用分类
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("status/{status}")]
        public async Task<ActionResult<ApiResult>> StartOrStop(int status, long id)
        {
            if (id <= 0)
            {
                return ApiResultHelper.Error("分类id必须为正整数");
            }
            await _categoryService.StartOrStopAsync(status, id);
            return ApiResultHelper.Success();
        }
    }
}
