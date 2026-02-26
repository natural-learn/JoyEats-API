using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Models;

namespace JoyEats.IRepository
{
    public interface IUserRepository : IBaseRepository<User>, IScopeDependency
    {
        /// <summary>
        /// 根据动态条件统计用户数量
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        Task<int> CountByMapAsync(Dictionary<string, object> map);
    }
}
