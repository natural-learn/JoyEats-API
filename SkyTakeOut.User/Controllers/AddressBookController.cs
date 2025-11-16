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

        /// <summary>
        /// 根据Id查询地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<AddressBook?>>> GetById(long id)
        {
            _logger.LogInformation("根据id:{@long} 查询地址", id);
            AddressBook? addressBook = await _addressBookService.GetByIdAsync(id);
            return ApiResultHelper.Success(addressBook);
        }

        /// <summary>
        /// 根据Id修改地址
        /// </summary>
        /// <param name="addressBook"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResult>> Update([FromBody] AddressBookDTO addressBookDTO)
        {
            await _addressBookService.UpdateAsync(addressBookDTO);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="addressBook"></param>
        /// <returns></returns>
        [HttpPut("default")]
        public async Task<ActionResult<ApiResult>> SetDefault([FromBody]AddressBook addressBook)
        {
            await _addressBookService.SetDefaultAsync(addressBook);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 根据Id删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<ApiResult>> DeleteById(long id)
        {
            _logger.LogInformation("删除指定Id:{@long}的地址", id);
            await _addressBookService.DeleteByIdAsync(id);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 查询默认地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("default")]
        public async Task<ActionResult<ApiResult<AddressBook>>> GetDefault()
        {
            AddressBook addressBook = new AddressBook()
            {
                IsDefault =1,
            };
            List<AddressBook> addressBookList = await _addressBookService.ListAsync(addressBook);
            if (addressBookList != null && addressBookList.Count == 1)
            {
                return ApiResultHelper.Success(addressBookList[0]);
            }
            return ApiResultHelper.Error<AddressBook>("没有查询到默认地址");
        }
    }
}
