using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Core.DTO.ShoppingCart;
using SkyTakeOut.Models;

namespace SkyTakeOut.IServices
{
    public interface IShoppingCartService : IScopeDependency
    {
        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="shoppingCartDTO"></param>
        /// <returns></returns>
        Task AddShoppingCartAsync(ShoppingCartDTO shoppingCartDTO);

        /// <summary>
        /// 查看购物车
        /// </summary>
        /// <returns></returns>
        Task<List<ShoppingCart>> ShowShoppingCartAsync();

        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <returns></returns>
        Task CleanShoppingCartAsync();
    }
}
