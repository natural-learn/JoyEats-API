using JoyEats.Common;
using JoyEats.Core.DTO.AddressBook;
using JoyEats.IServices;
using JoyEats.Models;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.User.Controllers
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
        /// <param name="addressBookDTO"></param>
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
        /// <param name="addressBookDTO"></param>
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
        /// <param name="addressBookDefaultDTO"></param>
        /// <returns></returns>
        [HttpPut("default")]
        public async Task<ActionResult<ApiResult>> SetDefault([FromBody]AddressBookDefaultDTO addressBookDefaultDTO)
        {
            await _addressBookService.SetDefaultAsync(addressBookDefaultDTO.Id);
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
            AddressBook? addressBook = await _addressBookService.GetDefaultAsync();
            if (addressBook != null)
            {
                return ApiResultHelper.Success(addressBook);
            }
            return ApiResultHelper.Error<AddressBook>("没有查询到默认地址");
        }
    }
}
