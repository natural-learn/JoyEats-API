using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Core.DTO.User;
using JoyEats.Models;

namespace JoyEats.IServices
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
