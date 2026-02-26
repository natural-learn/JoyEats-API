using JoyEats.Common;
using JoyEats.Core.VO.Dashboard;
using JoyEats.IServices;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.Admin.Controllers
{
    [Route("admin/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// 今日数据查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("businessData")]
        public async Task<ActionResult<ApiResult<BusinessDataVO>>> BusinessData()
        {
            //获得当天的开始时间
            DateTime begin = DateTime.Today;
            //获得当天的结束时间
            DateTime end = DateTime.Today.AddDays(1).AddTicks(-1);

            BusinessDataVO businessDataDto = await _dashboardService.GetBusinessDataAsync(begin, end);
            return ApiResultHelper.Success(businessDataDto);
        }

        /// <summary>
        /// 查询订单管理数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("overviewOrders")]
        public async Task<ActionResult<ApiResult<OrderOverViewVO>>> OrderOverView()
        {
            OrderOverViewVO orderOverViewDto = await _dashboardService.GetOrderOverViewAsync();
            return ApiResultHelper.Success(orderOverViewDto);
        }

        /// <summary>
        /// 查询菜品总览
        /// </summary>
        /// <returns></returns>
        [HttpGet("overviewDishes")]
        public async Task<ActionResult<ApiResult<DishOverViewVO>>> DishOverView()
        {
            DishOverViewVO dishOverViewDto = await _dashboardService.GetDishOverViewAsync();
            return ApiResultHelper.Success(dishOverViewDto);
        }
    }
}
