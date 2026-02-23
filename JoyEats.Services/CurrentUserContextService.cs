using JoyEats.Common;
using JoyEats.Common.Constant;
using JoyEats.Common.Helpers.Redis;
using JoyEats.IServices;

namespace JoyEats.Services
{
    public class CurrentUserContextService : ICurrentUserContextService
    {
        /// <summary>
        /// 获取当前用户Id
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetCurrentUserIdAsync()
        {
            try
            {
                var userIdString = await StackExchangeRedisHelper.StringGetAsync(RedisConstant.UserId);
                return long.Parse(userIdString);
            }
            catch (Exception)
            {
                throw new BusinessException("用户未登录或登录已过期");
            }
        }
    }
}
