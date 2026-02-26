using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Models;

namespace JoyEats.IRepository
{
    public interface ISetmealRepository : IBaseRepository<Setmeal>, IScopeDependency
    {
        /// <summary>
        /// 根据条件统计套餐数量
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        Task<int> CountByMapAsync(Dictionary<string, object> map);
    }
}
