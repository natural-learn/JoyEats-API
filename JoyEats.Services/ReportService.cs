using JoyEats.Core.VO.Report;
using JoyEats.IRepository;
using JoyEats.IRepository.UnitOfWork;
using JoyEats.IServices;
using JoyEats.Models;
using System.Linq.Expressions;

namespace JoyEats.Services
{
    public class ReportService : BaseService, IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IBaseRepository<User> _userRepository;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = _unitOfWork.GetRepository<IOrderRepository>();
            _userRepository = _unitOfWork.GetBaseRepository<User>();
        }

        /// <summary>
        /// 营业额数据统计
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<TurnoverReportVO> GetTurnoverAsync(DateTime beginTime, DateTime endTime)
        {
            List<DateTime> dateList = new List<DateTime>();
            dateList.Add(beginTime);

            while (beginTime < endTime)
            {
                beginTime = beginTime.AddDays(1);
                dateList.Add(endTime);
            }

            List<decimal> turnoverList = new List<decimal>();
            foreach (DateTime date in dateList)
            {
                DateTime begin = date.Date;
                DateTime end = date.Date.AddDays(1).AddTicks(-1);
                Dictionary<string, object> map = new Dictionary<string, object>();
                map.Add("status", OrderStatus.COMPLETED);
                map.Add("begin", begin);
                map.Add("end", end);
                decimal turnover = await _orderRepository.SumByMapAsync(map);
                turnover = turnover == 0 ? 0.0m : turnover;
                turnoverList.Add(turnover);
            }

            return new TurnoverReportVO
            {
                DateList = string.Join(",", dateList),
                TurnoverList = string.Join(",", turnoverList)
            };
        }

        /// <summary>
        /// 根据时间区间统计用户数量
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<UserReportVO> GetUserStatisticsAsync(DateTime beginTime, DateTime endTime)
        {
            List<DateTime> dateList = new List<DateTime>();
            dateList.Add(beginTime);

            while (beginTime < endTime)
            {
                beginTime = beginTime.AddDays(1);
                dateList.Add(beginTime);
            }

            List<int> newUserList = new List<int>();
            List<int> totalUserList = new List<int>();

            foreach (DateTime date in dateList)
            {
                DateTime begin = date.Date;
                DateTime end = date.Date.AddDays(1).AddTicks(-1);
                //新增用户数量
                int newUser = await GetUserCountAsync(begin, end);
                int totalUser = await GetUserCountAsync(null, end);

                newUserList.Add(newUser);
                totalUserList.Add(totalUser);
            }

            return new UserReportVO
            {
                DateList = string.Join(",", dateList),
                NewUserList = string.Join(",", newUserList),
                TotalUserList = string.Join(",", totalUserList)
            };
        }

        /// <summary>
        /// 根据时间区间统计用户数量
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<int> GetUserCountAsync(DateTime? beginTime, DateTime? endTime)
        {
            Dictionary<string, DateTime?> map = new Dictionary<string, DateTime?>();
            map.Add("begin", beginTime);
            map.Add("end", endTime);

            Expression<Func<User, bool>> predicate = u => true;
            if (beginTime.HasValue)
            {
                predicate = predicate.And(u => u.CreateTime >= beginTime);
            }
            if (endTime.HasValue)
            {
                predicate = predicate.And(u => u.CreateTime <= endTime);
            }
            return await _userRepository.CountAsync(predicate);
        }

        /// <summary>
        /// 根据时间区间统计订单数量
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<OrderReportVO> GetOrderStatisticsAsync(DateTime beginTime, DateTime endTime)
        {
            List<DateTime> dateList = new List<DateTime>();
            dateList.Add(beginTime);

            while (beginTime < endTime)
            {
                beginTime = beginTime.AddDays(1);
                dateList.Add(beginTime);
            }

            //每天订单总数集合
            List<int> orderCountList = new List<int>();

            //每天有效订单数集合
            List<int> validOrderCountList = new List<int>();

            foreach (DateTime date in dateList)
            {
                DateTime begin = date.Date;
                DateTime end = date.Date.AddDays(1).AddTicks(-1);
                int orderCount = await GetOrderCountAsync(begin, end, null);
                int validOrderCount = await GetOrderCountAsync(begin, end, OrderStatus.COMPLETED);
                orderCountList.Add(validOrderCount);
                validOrderCountList.Add(validOrderCount);
            }

            //时间区间内的总订单数
            int totalOrderCount = orderCountList.Sum();
            //时间区间内的总有效订单数
            int validTotalOrderCount = validOrderCountList.Sum();
            //订单完成率
            double orderCompletionRate = 0.0;
            if (totalOrderCount != 0)
            {
                orderCompletionRate = totalOrderCount == 0 ? 0 : (double)validTotalOrderCount / totalOrderCount;
            }

            return new OrderReportVO
            {
                DateList = string.Join(",", dateList),
                OrderCountList = string.Join(",", orderCountList),
                ValidOrderCountList = string.Join(",", validOrderCountList),
                TotalOrderCount = totalOrderCount,
                ValidOrderCount = validTotalOrderCount,
                OrderCompletionRate = orderCompletionRate
            };
        }

        private async Task<int> GetOrderCountAsync(DateTime begin, DateTime end, int? status)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add("status", status);
            map.Add("begin", begin);
            map.Add("end", end);
            return await _orderRepository.CountByMapAsync(map);
        }
    }
}
