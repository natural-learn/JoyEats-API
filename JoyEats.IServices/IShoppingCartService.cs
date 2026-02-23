using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Core.DTO.ShoppingCart;
using JoyEats.Models;

namespace JoyEats.IServices
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
