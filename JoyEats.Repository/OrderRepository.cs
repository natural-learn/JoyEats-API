using JoyEats.Common;
using JoyEats.Core.DTO.Order;
using JoyEats.Core.DTO.Report;
using JoyEats.EntityFrameworkCore;
using JoyEats.IRepository;
using JoyEats.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JoyEats.Repository
{
    public class OrderRepository : EFCoreRepository<Orders>, IOrderRepository
    {
        private readonly DbSet<OrderDetail> orderDetail;
        private readonly DbSet<Orders> orders;

        public OrderRepository(AppDbContext dbContext)
            : base(dbContext)
        {
            orderDetail = dbContext.Set<OrderDetail>();
            orders = dbContext.Set<Orders>();
        }

        /// <summary>
        /// 分页条件查询并按下单时间排序
        /// </summary>
        /// <param name="ordersPageQueryDTO"></param>
        /// <returns></returns>
        public async Task<PagedResult<Orders>> PageQueryAsync(OrdersPageQueryDTO ordersPageQueryDTO)
        {
            Expression<Func<Orders, bool>> predicate = o => o.UserId == ordersPageQueryDTO.UserId;

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

            var query = GetQueryable().Where(predicate);

            int totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return PagedResult<Orders>.GetPagedResult(new List<Orders>(), ordersPageQueryDTO.Page, ordersPageQueryDTO.PageSize, totalCount);
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

        /// <summary>
        /// 根据动态条件统计订单数量
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public async Task<int> CountByMapAsync(Dictionary<string, object> map)
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
                .CountAsync();
        }

        /// <summary>
        /// 查询商品销量排名
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<List<GoodsSalesDTO>> GetSalesTop10Async(DateTime? beginTime, DateTime? endTime)
        {
            var query = from od in orderDetail
                        join o in orders on od.OrderId equals o.Id
                        where o.Status == 5
                        where !beginTime.HasValue || o.OrderTime >= beginTime
                        where !endTime.HasValue || o.OrderTime <= endTime
                        group od by od.Name into g
                        select new GoodsSalesDTO
                        {
                            Name = g.Key,
                            Number = g.Sum(od => od.Number),
                        }
                        into grouped
                        orderby grouped.Number descending
                        select grouped;
            return await query.Skip(0).Take(10).ToListAsync();
        }
    }
}
