using JoyEats.EntityFrameworkCore;
using JoyEats.IRepository;
using JoyEats.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JoyEats.Repository
{
    public class UserRepository : EFCoreRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) 
            : base(dbContext)
        {
        }

        /// <summary>
        /// 根据动态条件统计用户数量
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public async Task<int> CountByMapAsync(Dictionary<string, object> map)
        {
            DateTime? beginTime = Convert.ToDateTime(map["begin"]);
            DateTime? endTime = Convert.ToDateTime(map["end"]);
            Expression<Func<User, bool>> predicate = u => true;
            if (beginTime.HasValue)
            {
                predicate = predicate.And(u => u.CreateTime >= beginTime);
            }
            if (endTime.HasValue)
            {
                predicate = predicate.And(u => u.CreateTime <= endTime);
            }
            return await GetQueryable()
                .Where(predicate)
                .CountAsync();
        }
    }
}
