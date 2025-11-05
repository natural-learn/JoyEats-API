using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Common.Helpers;
using SkyTakeOut.Core.DTO.User;
using SkyTakeOut.Core.VO.User;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;

namespace SkyTakeOut.User.Controllers
{
    [Route("user/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly JWTHelper _jWTHelper;

        public UserController(ILogger<UserController> logger, IUserService userService, JWTHelper jWTHelper)
        {
            _logger = logger;
            _userService = userService;
            _jWTHelper = jWTHelper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResult<UserLoginVo>>> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            _logger.LogInformation("微信用户登录：{@UserLoginDTO}", userLoginDTO);

            Models.User? user = null;
            try
            {
                user = await _userService.WxLoginAsync(userLoginDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "微信用户登录异常：{@UserLoginDTO}", userLoginDTO);
                return ApiResultHelper.Error<UserLoginVo>(ex.Message);
            }

            if (user == null)
            {
                return ApiResultHelper.Error<UserLoginVo>("登录失败");
            }

            string token = _jWTHelper.CreateToken(user.Id.ToString(), user.Name, "user");
            UserLoginVo userLoginVo = new UserLoginVo
            {
                Id = user.Id,
                OpenId = user.Openid,
                Token = token
            };

            return ApiResultHelper.Success(userLoginVo);
        }
    }
}
