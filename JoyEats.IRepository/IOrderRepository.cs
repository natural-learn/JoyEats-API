using JoyEats.Common;
using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Core.DTO.Order;
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
    }
}
