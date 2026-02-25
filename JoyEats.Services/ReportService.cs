using JoyEats.Core.VO.Report;
using JoyEats.IRepository;
using JoyEats.IRepository.UnitOfWork;
using JoyEats.IServices;
using JoyEats.Models;

namespace JoyEats.Services
{
    public class ReportService : BaseService, IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = _unitOfWork.GetRepository<IOrderRepository>();
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
    }
}
