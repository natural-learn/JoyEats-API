using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Core.DTO.Dish;
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
    }
}
