using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Models;

namespace SkyTakeOut.IRepository
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
