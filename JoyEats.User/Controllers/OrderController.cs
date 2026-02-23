using JoyEats.Common;
using JoyEats.Core.DTO.Order;
using JoyEats.Core.VO.Order;
using JoyEats.IServices;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.User.Controllers
{
    /// <summary>
    /// C端订单接口
    /// </summary>
    [Route("user/[controller]")]
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
        /// 用户下单
        /// </summary>
        /// <param name="orderSubmitDTO"></param>
        /// <returns></returns>
        [HttpPost("submit")]
        public async Task<ActionResult<ApiResult<OrderSubmitVO>>> Submit([FromBody] OrdersSubmitDTO orderSubmitDTO)
        {
            _logger.LogInformation("用户下单：{@OrdersSubmitDTO}", orderSubmitDTO);
            OrderSubmitVO orderSubmitVO = await _orderService.SubmitOrderAsync(orderSubmitDTO);
            return ApiResultHelper.Success(orderSubmitVO);
        }

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="ordersPaymentDTO"></param>
        /// <returns></returns>
        [HttpPut("payment")]
        public async Task<ActionResult<ApiResult<OrderPaymentVO>>> Payment([FromBody]OrdersPaymentDTO ordersPaymentDTO)
        {
            _logger.LogInformation("订单支付：{@OrdersPaymentDTO}", ordersPaymentDTO);
            OrderPaymentVO orderPaymentVO = await _orderService.PaymentAsync(ordersPaymentDTO);
            _logger.LogInformation("生成预支付交易单：{@OrderPaymentVO}", orderPaymentVO);
            return ApiResultHelper.Success(orderPaymentVO);
        }

        /// <summary>
        /// 历史订单查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="status">订单状态 1待付款 2待接单 3已接单 4派送中 5已完成 6已取消</param>
        /// <returns></returns>
        [HttpGet("historyOrders")]
        public async Task<ActionResult<ApiResult<PagedResult<OrderVO>>>> Page(int page, int pageSize, int? status)
        {
            PagedResult<OrderVO> pagedResult = await _orderService.PageQueryUserAsync(page, pageSize, status);
            return ApiResultHelper.Success(pagedResult);
        }

        /// <summary>
        /// 查询订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("orderDetail/{id}")]
        public async Task<ActionResult<ApiResult<OrderVO>>> Details(long id)
        {
            OrderVO orderVO = await _orderService.DetailsAsync(id);
            return ApiResultHelper.Success(orderVO); 
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("cancel/{id}")]
        public async Task<ActionResult<ApiResult>> Cancel(long id)
        {
            await _orderService.UserCancelByIdAsync(id);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 再来一单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("repetition/{id}")]
        public async Task<ActionResult<ApiResult>> Repetition(long id)
        {
            await _orderService.RepetitionAsync(id);
            return ApiResultHelper.Success();
        }
    }
}
