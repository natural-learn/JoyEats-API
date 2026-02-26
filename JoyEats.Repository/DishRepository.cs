using JoyEats.EntityFrameworkCore;
using JoyEats.IRepository;
using JoyEats.Models;
using Microsoft.EntityFrameworkCore;

namespace JoyEats.Repository
{
    public class DishRepository : EFCoreRepository<Dish>, IDishRepository
    {
        private readonly AppDbContext _dbContext;

        public DishRepository(AppDbContext dbContext) 
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 根据套餐id查询菜品
        /// </summary>
        /// <param name="setmealId"></param>
        /// <returns></returns>
        public async Task<List<Dish>> GetBySetmealIdAsync(long setmealId)
        {
            return await _dbContext.Set<SetmealDish>()
                .Where(sd => sd.SetmealId == setmealId)
                .Select(sd => sd.Dish)
                .ToListAsync();
        }
    }
}
