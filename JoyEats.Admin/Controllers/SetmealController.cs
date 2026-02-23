using JoyEats.Common;
using JoyEats.Core.DTO.Setmeal;
using JoyEats.Core.VO.Setmeal;
using JoyEats.IServices;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.Admin.Controllers
{
    [Route("admin/[controller]")]
    [ApiController]
    public class SetmealController : ControllerBase
    {
        private readonly ILogger<SetmealController> _logger;
        private readonly ISetmealService _setmealService;

        public SetmealController(ILogger<SetmealController> logger, ISetmealService setmealService)
        {
            _logger = logger;
            _setmealService = setmealService;
        }

        /// <summary>
        /// 新增套餐
        /// </summary>
        /// <param name="setmealDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResult>> Save([FromBody] SetmealDTO setmealDTO)
        {
            _logger.LogInformation("新增套餐：{@SetmealDTO}", setmealDTO);
            await _setmealService.SaveWithDishAsync(setmealDTO);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="setmealPageQueryDTO"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<ActionResult<ApiResult<PagedResult<SetmealVo>>>> Page([FromQuery] SetmealPageQueryDTO setmealPageQueryDTO)
        {
            _logger.LogInformation("分页查询套餐：{@SetmealPageQueryDTO}", setmealPageQueryDTO);
            PagedResult<SetmealVo> pagedResult = await _setmealService.PageQueryAsync(setmealPageQueryDTO);
            return ApiResultHelper.Success(pagedResult);
        }

        /// <summary>
        /// 套餐启售停售
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("status/{status}")]
        public async Task<ActionResult<ApiResult>> StartOrStop(int status, long id)
        {
            await _setmealService.StartOrStopAsync(status, id);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 根据Id查询套餐信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<SetmealVo>>> GetById(long id)
        {
            _logger.LogInformation("根据id查询套餐信息：{Id}", id);
            SetmealVo setmealVo = await _setmealService.GetByIdWithDishAsync(id);
            return ApiResultHelper.Success(setmealVo);
        }

        /// <summary>
        /// 修改套餐
        /// </summary>
        /// <param name="setmealDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResult>> Update([FromBody] SetmealDTO setmealDTO)
        {
            _logger.LogInformation("修改套餐：{@SetmealDTO}", setmealDTO);
            try
            {
                await _setmealService.UpdateSetmealAsync(setmealDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError("{@string}", ex.Message);
                return ApiResultHelper.Error("修改套餐失败");
            }
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 批量删除套餐
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<ApiResult>> Delete([FromQuery] List<long> ids)
        {
            _logger.LogInformation("删除套餐：{@List<long>}", ids);
            try
            {
                await _setmealService.DeleteBatchAsync(ids);
            }
            catch (Exception ex)
            {
                _logger.LogError("{@string}", ex.Message);
                return ApiResultHelper.Error(ex.Message);
            }
            return ApiResultHelper.Success();
        }
    }
}
