using JoyEats.Common;
using JoyEats.Core.DTO.ShoppingCart;
using JoyEats.IServices;
using JoyEats.Models;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.User.Controllers
{
    /// <summary>
    /// C端购物车接口
    /// </summary>
    [Route("user/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(ILogger<ShoppingCartController> logger,IShoppingCartService shoppingCartService)
        {
            _logger = logger;
            _shoppingCartService = shoppingCartService;
        }

        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="shoppingCartDTO"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ActionResult<ApiResult>> Add([FromBody] ShoppingCartDTO shoppingCartDTO)
        {
            _logger.LogInformation("添加购物车：{@ShoppingCartDTO}", shoppingCartDTO);
            await _shoppingCartService.AddShoppingCartAsync(shoppingCartDTO);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 查看购物车
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult<ApiResult<List<ShoppingCart>>>> List()
        {
            _logger.LogInformation("查看购物车");
            List<ShoppingCart> shoppingCartList = await _shoppingCartService.ShowShoppingCartAsync();
            return ApiResultHelper.Success(shoppingCartList);
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <returns></returns>
        [HttpDelete("clean")]
        public async Task<ActionResult<ApiResult>> Clean()
        {
            _logger.LogInformation("清空购物车");
            await _shoppingCartService.CleanShoppingCartAsync();
            return ApiResultHelper.Success();
        }
    }
}
