using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Models;

namespace JoyEats.IRepository
{
    public interface IShoppingCartRepository : IBaseRepository<ShoppingCart>, IScopeDependency
    {
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        Task<List<ShoppingCart>> ListAsync(ShoppingCart shoppingCart);
    }
}
