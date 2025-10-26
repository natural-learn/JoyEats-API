using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Core.DTO.Dish;
using SkyTakeOut.Core.VO.Dish;
using SkyTakeOut.IServices;

namespace SkyTakeOut.Admin.Controllers
{
    /// <summary>
    /// 菜品管理
    /// </summary>
    [Route("admin/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly ILogger<DishController> _logger;
        private readonly IDishService _dishService;

        public DishController(ILogger<DishController> logger, IDishService dishService)
        {
            _logger = logger;
            _dishService = dishService;
        }

        /// <summary>
        /// 新增菜品
        /// </summary>
        /// <param name="dishDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResult>> Save([FromBody] DishDTO dishDTO)
        {
            _logger.LogInformation("新增菜品：{@DishDTO}", dishDTO);
            await _dishService.SaveWithFlavorAsync(dishDTO);

            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 菜品分页查询
        /// </summary>
        /// <param name="dishPageQueryDTO"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<ActionResult<ApiResult<PagedResult<DishVo>>>> Page([FromQuery] DishPageQueryDTO dishPageQueryDTO)
        {
            _logger.LogInformation("菜品分页查询：{@DishPageQueryDTO}", dishPageQueryDTO);
            var pagedResult = await _dishService.PageQueryAsync(dishPageQueryDTO);
            return ApiResultHelper.Success(pagedResult);
        }
    }
}
