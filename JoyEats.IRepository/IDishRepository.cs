using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Models;

namespace JoyEats.IRepository
{
    public interface IDishRepository : IBaseRepository<Dish>, IScopeDependency
    {
        /// <summary>
        /// 根据套餐id查询菜品
        /// </summary>
        /// <param name="setmealId"></param>
        /// <returns></returns>
        public Task<List<Dish>> GetBySetmealIdAsync(long setmealId);

        /// <summary>
        /// 根据条件统计菜品数量
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        Task<int> CountByMapAsync(Dictionary<string, object> map);
    }
}
