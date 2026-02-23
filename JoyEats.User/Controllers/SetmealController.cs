using JoyEats.Common;
using JoyEats.Common.Constant;
using JoyEats.Core.DTO.Setmeal;
using JoyEats.Core.VO.Dish;
using JoyEats.IServices;
using JoyEats.Models;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.User.Controllers
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
