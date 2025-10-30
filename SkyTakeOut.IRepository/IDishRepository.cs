using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Models;

namespace SkyTakeOut.IRepository
{
    public interface IDishRepository : IBaseRepository<Dish>, IScopeDependency
    {
        /// <summary>
        /// 根据套餐id查询菜品
        /// </summary>
        /// <param name="setmealId"></param>
        /// <returns></returns>
        public Task<List<Dish>> GetBySetmealIdAsync(long setmealId);
    }
}
