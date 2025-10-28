using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Core.DTO.Setmeal;
using SkyTakeOut.Core.VO.Setmeal;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;

namespace SkyTakeOut.Admin.Controllers
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
    }
}
