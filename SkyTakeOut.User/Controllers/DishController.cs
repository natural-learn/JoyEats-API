using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Common.Constant;
using SkyTakeOut.Core.VO.Dish;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;

namespace SkyTakeOut.User.Controllers
{
    /// <summary>
    /// C端-菜品浏览接口
    /// </summary>
    [Route("user/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        /// <summary>
        /// 根据分类id查询菜品
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult<ApiResult<List<DishVo>>>> List(long categoryId)
        {
            Dish dish = new Dish() { CategoryId = categoryId, Status = StatusConstant.ENABLE };
            List<DishVo> dishVoList = await _dishService.ListWithFlavorAsync(dish);
            return ApiResultHelper.Success(dishVoList);
        }
    }
}
