using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Common.Helpers.Redis;

namespace SkyTakeOut.Admin.Controllers
{
    /// <summary>
    /// 店铺相关接口
    /// </summary>
    [Route("admin/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private static readonly string KEY = "SHOP_STATUS";
        private readonly ILogger<ShopController> _logger;

        public ShopController(ILogger<ShopController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 设置店铺的营业状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("{status}")]
        public async Task<ActionResult<ApiResult>> SetStatus(int status)
        {
            _logger.LogInformation("设置店铺的营业状态为：{@int}", status == 1 ? "营业中" : "打烊中");
            await StackExchangeRedisHelper.StringSetAsync(KEY, status.ToString());
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 获取店铺的营业状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("status")]
        public async Task<ActionResult<ApiResult<int>>> GetStatus()
        {
            var statusString = await StackExchangeRedisHelper.StringGetAsync(KEY);
            int status = int.Parse(statusString);
            _logger.LogInformation("获取到店铺的营业状态为：{@int}", status == 1 ? "营业中" : "打烊中");
            return ApiResultHelper.Success(status);
        }
    }
}
