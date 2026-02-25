using JoyEats.Common;
using JoyEats.Core.DTO.Order;
using JoyEats.Core.VO.Order;
using JoyEats.IServices;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.Admin.Controllers
{
    /// <summary>
    /// 订单管理接口
    /// </summary>
    [Route("admin/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        /// <summary>
        /// 订单搜索
        /// </summary>
        /// <param name="ordersPageQueryDTO"></param>
        /// <returns></returns>
        [HttpGet("conditionSearch")]
        public async Task<ActionResult<ApiResult<PagedResult<OrderVO>>>> ConditionSearch([FromQuery]OrdersPageQueryDTO ordersPageQueryDTO)
        {
            PagedResult<OrderVO> pagedResult = await _orderService.ConditionSearchAsync(ordersPageQueryDTO);
            return ApiResultHelper.Success(pagedResult);
        }

        /// <summary>
        /// 各个状态的订单数量统计
        /// </summary>
        /// <returns></returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<ApiResult<OrderStatisticsVo>>> Statistics()
        {
            OrderStatisticsVo orderStatisticsDto = await _orderService.StatisticsAsync();
            return ApiResultHelper.Success(orderStatisticsDto);
        }

        /// <summary>
        /// 查询订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("details/{id}")]
        public async Task<ActionResult<ApiResult<OrderVO>>> Details(long id)
        {
            OrderVO orderDto = await _orderService.DetailsAsync(id);
            return ApiResultHelper.Success(orderDto);
        }

        /// <summary>
        /// 接单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("confirm")]
        public async Task<ActionResult<ApiResult>> Confirm([FromBody] OrdersConfirmDTO ordersConfirmDTO)
        {
            await _orderService.ConfirmAsync(ordersConfirmDTO);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 拒单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("rejection")]
        public async Task<ActionResult<ApiResult>> Rejection([FromBody] OrdersRejectionDTO ordersRejectionDTO)
        {
            await _orderService.RejectionAsync(ordersRejectionDTO);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("cancel")]
        public async Task<ActionResult<ApiResult>> Cancel([FromBody] OrdersCancelDTO ordersCancelDTO)
        {
            await _orderService.CancelAsync(ordersCancelDTO);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 派送订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delivery/{id}")]
        public async Task<ActionResult<ApiResult>> Delivery(long id)
        {
            await _orderService.DeliveryAsync(id);
            return ApiResultHelper.Success();
        }
    }
}
