using JoyEats.EntityFrameworkCore;
using JoyEats.IRepository;
using JoyEats.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JoyEats.Repository
{
    public class SetmealRepository : EFCoreRepository<Setmeal>, ISetmealRepository
    {
        public SetmealRepository(AppDbContext dbContext) 
            : base(dbContext)
        {
        }

        /// <summary>
        /// 根据条件统计套餐数量
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public Task<int> CountByMapAsync(Dictionary<string, object> map)
        {
            int? status = Convert.ToInt32(map["status"]);
            long? categoryId = Convert.ToInt64(map["categoryId"]);

            Expression<Func<Setmeal, bool>> predicate = s => true;
            if (status.HasValue)
            {
                predicate = predicate.And(s => s.Status == status);
            }

            if (categoryId.HasValue)
            {
                predicate = predicate.And(s => s.CategoryId == categoryId);
            }

            return GetQueryable()
                .Where(predicate)
                .CountAsync();
        }
    }
}
