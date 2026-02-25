using JoyEats.Common;
using JoyEats.Core.DTO.Order;
using JoyEats.EntityFrameworkCore;
using JoyEats.IRepository;
using JoyEats.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SkyTakeOut.Repository
{
    public class OrderRepository : EFCoreRepository<Orders>, IOrderRepository
    {
        public OrderRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// 分页条件查询并按下单时间排序
        /// </summary>
        /// <param name="ordersPageQueryDTO"></param>
        /// <returns></returns>
        public async Task<PagedResult<Orders>> PageQueryAsync(OrdersPageQueryDTO ordersPageQueryDTO)
        {
            Expression<Func<Orders, bool>> predicate = o => o.UserId == ordersPageQueryDTO.UserId;

            var query = GetQueryable().Where(predicate);
            if (query.Count() == 0)
            {
                return PagedResult<Orders>.GetPagedResult(new List<Orders>(), ordersPageQueryDTO.Page, ordersPageQueryDTO.PageSize, 0);
            }

            if (!string.IsNullOrEmpty(ordersPageQueryDTO.Number))
            {
                predicate = predicate.And(o => o.Number.Contains(ordersPageQueryDTO.Number));
            }

            if (!string.IsNullOrEmpty(ordersPageQueryDTO.Phone))
            {
                predicate = predicate.And(o => o.Phone.Contains(ordersPageQueryDTO.Phone));
            }

            if (ordersPageQueryDTO.Status.HasValue)
            {
                predicate = predicate.And(o => o.Status == ordersPageQueryDTO.Status.Value);
            }

            if (ordersPageQueryDTO.BeginTime.HasValue)
            {
                predicate = predicate.And(o => o.OrderTime >= ordersPageQueryDTO.BeginTime.Value);
            }

            if (ordersPageQueryDTO.EndTime.HasValue)
            {
                predicate = predicate.And(o => o.OrderTime <= ordersPageQueryDTO.EndTime.Value);
            }

            var orderList = await query.Where(predicate).OrderByDescending(o => o.OrderTime).ToListAsync();
            return PagedResult<Orders>.GetPagedResult(orderList, ordersPageQueryDTO.Page, ordersPageQueryDTO.PageSize, orderList.Count);
        }

        /// <summary>
        /// 根据动态条件统计营业额
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public async Task<decimal> SumByMapAsync(Dictionary<string, object> map)
        {
            int? status = Convert.ToInt32(map["status"]);
            DateTime? beginTime = Convert.ToDateTime(map["begin"]);
            DateTime? endTime = Convert.ToDateTime(map["end"]);
            Expression<Func<Orders, bool>> predicate = o => true;
            if (status.HasValue)
            {
                predicate = predicate.And(o => o.Status == status);
            }
            if (beginTime.HasValue)
            {
                predicate = predicate.And(o => o.OrderTime >= beginTime);
            }
            if (endTime.HasValue)
            {
                predicate = predicate.And(o => o.OrderTime <= endTime);
            }
            return await GetQueryable()
                .Where(predicate)
                .SumAsync(o => o.Amount);
        }
    }
}
