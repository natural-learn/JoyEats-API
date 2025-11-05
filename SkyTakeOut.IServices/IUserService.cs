using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Core.DTO.User;
using SkyTakeOut.Models;

namespace SkyTakeOut.IServices
{
    public interface IUserService : IScopeDependency
    {
        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="userLoginDTO"></param>
        /// <returns></returns>
        Task<User?> WxLoginAsync(UserLoginDTO userLoginDTO);
    }
}
