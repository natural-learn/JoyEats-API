using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Core.DTO.AddressBook;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;
using System.Text.Json;

namespace SkyTakeOut.User.Controllers
{
    /// <summary>
    /// C端地址簿接口
    /// </summary>
    [Route("user/[controller]")]
    [ApiController]
    public class AddressBookController : ControllerBase
    {
        private readonly ILogger<AddressBookController> _logger;
        private readonly IAddressBookService _addressBookService;

        public AddressBookController(ILogger<AddressBookController> logger, IAddressBookService addressBookService)
        {
            _logger = logger;
            _addressBookService = addressBookService;
        }

        /// <summary>
        /// 查询当前登录用户的所有地址信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult<ApiResult<List<AddressBook>>>> List()
        {
            _logger.LogInformation("查询当前登录用户的所有地址信息");
            AddressBook addressBook = new AddressBook();
            List<AddressBook> addressBookList = await _addressBookService.ListAsync(addressBook);
            return ApiResultHelper.Success(addressBookList);
        }

        /// <summary>
        /// 新增地址
        /// </summary>
        /// <param name="addressBook"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResult>> Save([FromBody] AddressBookDTO addressBookDTO)
        {
            _logger.LogInformation("新增地址");
            await _addressBookService.SaveAsync(addressBookDTO);
            return ApiResultHelper.Success();
        }
    }
}
