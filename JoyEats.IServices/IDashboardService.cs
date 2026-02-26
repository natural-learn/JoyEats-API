using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Core.VO.Dashboard;

namespace JoyEats.IServices
{
    public interface IDashboardService : IScopeDependency
    {
        /// <summary>
        /// 根据时间段统计营业数据
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<BusinessDataVO> GetBusinessDataAsync(DateTime begin, DateTime end);

        /// <summary>
        /// 查询订单管理数据
        /// </summary>
        /// <returns></returns>
        Task<OrderOverViewVO> GetOrderOverViewAsync();

        /// <summary>
        /// 查询菜品总览
        /// </summary>
        /// <returns></returns>
        Task<DishOverViewVO> GetDishOverViewAsync();
    }
}
