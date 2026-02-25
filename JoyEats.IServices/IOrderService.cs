using JoyEats.Common;
using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Core.DTO.Order;
using JoyEats.Core.VO.Order;

namespace JoyEats.IServices
{
    public interface IOrderService : IScopeDependency
    {
        /// <summary>
        /// 用户下单
        /// </summary>
        /// <param name="ordersSubmitDTO"></param>
        /// <returns></returns>
        Task<OrderSubmitVO> SubmitOrderAsync(OrdersSubmitDTO ordersSubmitDTO);

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="ordersPaymentDTO"></param>
        /// <returns></returns>
        Task<OrderPaymentVO> PaymentAsync(OrdersPaymentDTO ordersPaymentDTO);

        /// <summary>
        /// 历史订单查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="status">订单状态 1待付款 2待接单 3已接单 4派送中 5已完成 6已取消</param>
        /// <returns></returns>
        Task<PagedResult<OrderVO>> PageQueryUserAsync(int page, int pageSize, int? status);

        /// <summary>
        /// 查询订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrderVO> DetailsAsync(long id);

        /// <summary>
        /// 用户取消订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UserCancelByIdAsync(long id);

        /// <summary>
        /// 再来一单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RepetitionAsync(long id);

        /// <summary>
        /// 订单搜索
        /// </summary>
        /// <param name="ordersPageQueryDTO"></param>
        /// <returns></returns>
        Task<PagedResult<OrderVO>> ConditionSearchAsync(OrdersPageQueryDTO ordersPageQueryDTO);

        /// <summary>
        /// 各个状态的订单数量统计
        /// </summary>
        /// <returns></returns>
        Task<OrderStatisticsVo> StatisticsAsync();

        /// <summary>
        /// 接单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task ConfirmAsync(OrdersConfirmDTO ordersConfirmDTO);

        /// <summary>
        /// 拒单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task RejectionAsync(OrdersRejectionDTO ordersRejectionDTO);

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task CancelAsync(OrdersCancelDTO ordersCancelDTO);

        /// <summary>
        /// 派送订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeliveryAsync(long id);

        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task CompleteAsync(long id);
    }
}
