using JoyEats.Common;
using JoyEats.Common.Configs;
using JoyEats.Common.Constant;
using JoyEats.Core.DTO.Responses;
using JoyEats.Core.DTO.User;
using JoyEats.Core.Exceptions;
using JoyEats.IRepository;
using JoyEats.IRepository.UnitOfWork;
using JoyEats.IServices;
using JoyEats.Models;
using Microsoft.Extensions.Options;

namespace JoyEats.Services
{
    public class UserService : IUserService
    {
        private const string WX_LOGIN = "https://api.weixin.qq.com/sns/jscode2session";

        private readonly IBaseRepository<User> _userRepository;
        private readonly WeChatSettings _weChatSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientService _httpClientService;

        public UserService(IUnitOfWork unitOfWork, IHttpClientService httpClientService, IOptions<WeChatSettings> options)
        {
            _userRepository = unitOfWork.GetBaseRepository<User>();
            _weChatSettings = options.Value;
            _unitOfWork = unitOfWork;
            _httpClientService = httpClientService;
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="userLoginDTO"></param>
        /// <returns></returns>
        public async Task<User?> WxLoginAsync(UserLoginDTO userLoginDTO)
        {
            string? openId = await GetOpenIdAsync(userLoginDTO.Code);

            if (string.IsNullOrWhiteSpace(openId))
            {
                throw new LoginFailedException(MessageConstant.LOGIN_FAILED);
            }

            //openid不为空，说明是合法的微信用户，判断当前微信用户是否是新的用户
            User? user = await _userRepository.SingleOrDefaultAsync(u => u.Openid == openId);

            // 如果是新用户，自动注册
            if (user != null)
            {
                return user;
            }

            user = new User()
            {
                Openid = openId,
            };

            try
            {
                await _userRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new BusinessException("用户注册失败，请重试");
            }

            return user;
        }

        /// <summary>
        /// 获取Openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<string?> GetOpenIdAsync(string code)
        {
            //调用微信接口服务获得当前微信用户的openid
            Dictionary<string, object> map = new Dictionary<string, object>
            {
                { "appid", _weChatSettings.AppId },
                { "secret", _weChatSettings.Secret },
                { "js_code", code },
                { "grant_type", "authorization_code" }
            };

            WeChatSessionResponse weChatSessionResponse = await _httpClientService.GetAsync<WeChatSessionResponse>(WX_LOGIN, map);
            return weChatSessionResponse.OpenId;
        }
    }
}
