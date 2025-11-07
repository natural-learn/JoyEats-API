using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Common.Constant;
using SkyTakeOut.Common.Helpers.Redis;
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
            string key = "dish_" + categoryId;
            // 查询Redis中是否存在菜品数据
            List<DishVo> dishVoList = await StackExchangeRedisHelper.GetAsync<List<DishVo>>(key);
            if (dishVoList != null && dishVoList.Count > 0)
            {
                // 如果存在，直接返回，无须查询数据库
                return ApiResultHelper.Success(dishVoList);
            }

            Dish dish = new Dish() { CategoryId = categoryId, Status = StatusConstant.ENABLE };
            dishVoList = await _dishService.ListWithFlavorAsync(dish);
            // 如果不存在，查询数据库，将查询到的数据放入Redis中
            await StackExchangeRedisHelper.SetAsync<List<DishVo>>(key, dishVoList);
            return ApiResultHelper.Success(dishVoList);
        }
    }
}
