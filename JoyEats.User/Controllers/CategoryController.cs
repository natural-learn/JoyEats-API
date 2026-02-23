using JoyEats.Common;
using JoyEats.IServices;
using JoyEats.Models;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.User.Controllers
{
    /// <summary>
    /// C端分类接口
    /// </summary>
    [Route("user/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// 查询分类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult<ApiResult<List<Category>>>> List(int type)
        {
            List<Category> categories = await _categoryService.ListAsync(type);
            return ApiResultHelper.Success(categories);
        }
    }
}
