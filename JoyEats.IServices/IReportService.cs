using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Core.VO.Report;

namespace JoyEats.IServices
{
    public interface IReportService : IScopeDependency
    {
        /// <summary>
        /// 营业额数据统计
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Task<TurnoverReportVO> GetTurnoverAsync(DateTime beginTime, DateTime endTime);

        /// <summary>
        /// 根据时间区间统计用户数量
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Task<UserReportVO> GetUserStatisticsAsync(DateTime beginTime, DateTime endTime);
    }
}
