using JoyEats.EntityFrameworkCore;
using JoyEats.IRepository;
using JoyEats.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JoyEats.Repository
{
    public class DishRepository : EFCoreRepository<Dish>, IDishRepository
    {
        public DishRepository(AppDbContext dbContext) 
            : base(dbContext)
        {
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

        /// <summary>
        /// 根据条件统计菜品数量
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public async Task<int> CountByMapAsync(Dictionary<string, object> map)
        {
            int? status = Convert.ToInt32(map["status"]);
            long? categoryId = Convert.ToInt64(map["categoryId"]);
            Expression<Func<Dish, bool>> predicate = d => true;
            if (status.HasValue)
            {
                predicate = predicate.And(d => d.Status == status);
            }
            if (categoryId.HasValue)
            {
                predicate = predicate.And(d => d.CategoryId == categoryId);
            }
            return await GetQueryable()
                .Where(predicate)
                .CountAsync();
        }
    }
}
