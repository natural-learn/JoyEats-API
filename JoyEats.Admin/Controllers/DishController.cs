using JoyEats.Common;
using JoyEats.Common.Helpers.Redis;
using JoyEats.Core.DTO.Dish;
using JoyEats.Core.VO.Dish;
using JoyEats.IServices;
using JoyEats.Models;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.Admin.Controllers
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
            // 清理缓存数据
            string key = "dish_" + dishDTO.CategoryId;
            await StackExchangeRedisHelper.CleanCacheAsync(key);

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

        /// <summary>
        /// 菜品批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<ApiResult>> Delete([FromQuery] string ids)
        {
            _logger.LogInformation("菜品批量删除：{@List<long>}", ids);
            List<long> idList = [];
            foreach (var str in ids.Split(','))
            {
                if (long.TryParse(str.Trim(), out long result))
                {
                    idList.Add(result);
                }
                else
                {
                    throw new InvalidOperationException("参数ids有非法的值");
                }
            }

            await _dishService.DeleteBatchAsync(idList);
            // 将所有的菜品缓存数据清理掉，所有以dish_开头的key
            await StackExchangeRedisHelper.CleanCacheAsync("dish_*");
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 菜品启售停售
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("status/{status}")]
        public async Task<ActionResult<ApiResult>> StartOrStop(int status, long id)
        {
            await _dishService.StartOrStopAsync(status, id);
            // 将所有的菜品缓存数据清理掉，所有以dish_开头的key
            await StackExchangeRedisHelper.CleanCacheAsync("dish_*");
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 根据Id查询菜品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<DishVo>>> GetById(long id)
        {
            _logger.LogInformation("根据Id查询菜品：{Id}", id);
            DishVo dishVo = await _dishService.GetByIdWithFlavorAsync(id);
            return ApiResultHelper.Success(dishVo);
        }

        /// <summary>
        /// 修改菜品
        /// </summary>
        /// <param name="dishDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResult>> Update([FromBody] DishDTO dishDTO)
        {
            _logger.LogInformation("修改菜品：{@DishDTO}", dishDTO);
            await _dishService.UpdateWithFlavorAsync(dishDTO);
            // 将所有的菜品缓存数据清理掉，所有以dish_开头的key
            await StackExchangeRedisHelper.CleanCacheAsync("dish_*");
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 根据分类id查询菜品
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult<ApiResult<List<Dish>>>> List(long categoryId)
        {
            _logger.LogInformation("根据分类id：{@long}查询菜品", categoryId);
            List<Dish> dishList = await _dishService.ListAsync(categoryId);
            return ApiResultHelper.Success(dishList);
        }
    }
}
