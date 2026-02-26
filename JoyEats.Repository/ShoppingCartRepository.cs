using JoyEats.EntityFrameworkCore;
using JoyEats.IRepository;
using JoyEats.Models;
using Microsoft.EntityFrameworkCore;

namespace JoyEats.Repository
{
    public class ShoppingCartRepository : EFCoreRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCart>> ListAsync(ShoppingCart shoppingCart)
        {
            ArgumentNullException.ThrowIfNull(shoppingCart, nameof(shoppingCart));
            var (userId, dishId, setmealId, dishFlavor) = (
                shoppingCart.UserId,
                shoppingCart.DishId,
                shoppingCart.SetmealId,
                shoppingCart.DishFlavor
            );
            IQueryable<ShoppingCart> query = GetQueryable().Where(sc => sc.UserId == userId);
            if (dishId.HasValue && dishId.Value > 0)
            {
                query = query.Where(sc => sc.DishId == dishId.Value);
            }

            if (setmealId.HasValue && setmealId.Value > 0)
            {
                query = query.Where(sc => sc.SetmealId == setmealId.Value);
            }
            if (!string.IsNullOrWhiteSpace(dishFlavor))
            {
                query = query.Where(sc => sc.DishFlavor == dishFlavor);
            }
            // ConfigureAwait(false)避免上下文切换，提升性能
            return await query.ToListAsync().ConfigureAwait(false);
        }
    }
}
