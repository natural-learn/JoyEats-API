using JoyEats.Common;
using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Core.DTO.Order;
using JoyEats.Core.DTO.Report;
using JoyEats.Models;

namespace JoyEats.IRepository
{
    public interface IOrderRepository : IBaseRepository<Orders>, IScopeDependency
    {
        /// <summary>
        /// 分页条件查询并按下单时间排序
        /// </summary>
        /// <param name="ordersPageQueryDTO"></param>
        /// <returns></returns>
        Task<PagedResult<Orders>> PageQueryAsync(OrdersPageQueryDTO ordersPageQueryDTO);

        /// <summary>
        /// 根据动态条件统计营业额
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        Task<decimal> SumByMapAsync(Dictionary<string, object> map);

        /// <summary>
        /// 根据动态条件统计订单数量
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        Task<int> CountByMapAsync(Dictionary<string, object> map);

        /// <summary>
        /// 查询商品销量排名
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Task<List<GoodsSalesDTO>> GetSalesTop10Async(DateTime? beginTime, DateTime? endTime);
    }
}
