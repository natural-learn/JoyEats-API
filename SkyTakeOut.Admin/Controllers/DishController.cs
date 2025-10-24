using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Core.DTO.Dish;

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

        public DishController(ILogger<DishController> logger)
        {
            _logger = logger;
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
            return ApiResultHelper.Success();
        }
    }
}
