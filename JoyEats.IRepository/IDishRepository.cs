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
    }
}
