using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Common.Constant;
using SkyTakeOut.Core.DTO.Setmeal;
using SkyTakeOut.Core.VO.Dish;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;

namespace SkyTakeOut.User.Controllers
{
    /// <summary>
    /// C端套餐浏览接口
    /// </summary>
    [Route("user/[controller]")]
    [ApiController]
    public class SetmealController : ControllerBase
    {
        private readonly ISetmealService _setmealService;

        public SetmealController(ISetmealService setmealService)
        {
            _setmealService = setmealService;
        }

        /// <summary>
        /// 根据分类Id查询套餐
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult<ApiResult<List<Setmeal>>>> List(long categoryId)
        {
            SetmealDTO setmealDTO = new SetmealDTO() { CategoryId = categoryId, Status = StatusConstant.ENABLE };
            List<Setmeal> setmeals = await _setmealService.ListAsync(setmealDTO);
            return ApiResultHelper.Success(setmeals);
        }

        /// <summary>
        /// 根据套餐Id查询包含的菜品列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("dish/{id}")]
        public async Task<ActionResult<ApiResult<List<DishItemVo>>>> DishList(long id)
        {
            List<DishItemVo> dishItemVos = await _setmealService.GetDishItemByIdAsync(id);
            return ApiResultHelper.Success(dishItemVos);
        }
    }
}
