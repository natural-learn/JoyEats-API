using JoyEats.Common;
using JoyEats.Common.Constant;
using JoyEats.Common.Helpers.Redis;
using JoyEats.Core.DTO.ShoppingCart;
using JoyEats.IRepository;
using JoyEats.IRepository.UnitOfWork;
using JoyEats.IServices;
using JoyEats.Models;
using Microsoft.EntityFrameworkCore;

namespace JoyEats.Services
{
    public class ShoppingCartService : BaseService, IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IBaseRepository<Dish> _dishRepository;
        private readonly IBaseRepository<Setmeal> _setmealRepository;

        private readonly long userId;

        public ShoppingCartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _shoppingCartRepository = _unitOfWork.GetRepository<IShoppingCartRepository>();
            _dishRepository = _unitOfWork.GetBaseRepository<Dish>();
            _setmealRepository = _unitOfWork.GetBaseRepository<Setmeal>();
            userId = long.Parse(StackExchangeRedisHelper.StringGetAsync(RedisConstant.UserId).Result);
        }

        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="shoppingCartDTO"></param>
        /// <returns></returns>
        public async Task AddShoppingCartAsync(ShoppingCartDTO shoppingCartDTO)
        {
            if (shoppingCartDTO == null)
            {
                throw new ArgumentNullException("购物车数据为空");
            }
            ShoppingCart shoppingCart = Mapper.Map<ShoppingCart>(shoppingCartDTO);
            try
            {
                // 只能查询自己的购物车数据
                shoppingCart.UserId = userId;

                // 判断当前商品是否已经存在购物车中
                List<ShoppingCart> shoppingCartList = await _shoppingCartRepository.ListAsync(shoppingCart);
                if (shoppingCartList.Count > 0)
                {
                    // 如果存在，更新数量+1
                    shoppingCart = shoppingCartList[0];
                    shoppingCart.Number += 1;
                    _shoppingCartRepository.Update(shoppingCart);
                }
                else
                {
                    // 如果不存在，插入数据，数量是1
                    // 判断当前添加到购物车的是菜品还是套餐
                    if (shoppingCartDTO.DishId.HasValue)
                    {
                        Dish? dish = await _dishRepository.GetByIdAsync(shoppingCartDTO.DishId) ??
                            throw new BusinessException("菜品不存在");
                        shoppingCart.Name = dish.Name;
                        shoppingCart.Image = dish.Image;
                        shoppingCart.Amount = dish.Price;
                    }
                    else if(shoppingCartDTO.SetmealId.HasValue)
                    {
                        Setmeal? setmeal = await _setmealRepository.GetByIdAsync(shoppingCartDTO.SetmealId.Value) ??
                            throw new BusinessException("套餐不存在");
                        shoppingCart.Name = setmeal.Name;
                        shoppingCart.Image = setmeal.Image;
                        shoppingCart.Amount = setmeal.Price;
                    }
                    shoppingCart.Number = 1;
                    shoppingCart.CreateTime = DateTime.Now;
                    await _shoppingCartRepository.AddAsync(shoppingCart);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessException($"添加购物车出错：{ex.Message}");
            }
        }

        /// <summary>
        /// 查看购物车
        /// </summary>
        /// <returns></returns>
        public async Task<List<ShoppingCart>> ShowShoppingCartAsync()
        {
            return await _shoppingCartRepository.ListAsync(new ShoppingCart { UserId = userId });
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <returns></returns>
        public async Task CleanShoppingCartAsync()
        {
            await _shoppingCartRepository
                .GetQueryable()
                .Where(sc => sc.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
