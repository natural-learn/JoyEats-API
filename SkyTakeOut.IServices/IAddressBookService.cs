using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Core.DTO.AddressBook;
using SkyTakeOut.Models;

namespace SkyTakeOut.IServices
{
    public interface IAddressBookService : IScopeDependency
    {
        /// <summary>
        /// 查询当前登录用户的所有地址信息
        /// </summary>
        /// <param name="addressBook"></param>
        /// <returns></returns>
        Task<List<AddressBook>> ListAsync(AddressBook addressBook);

        /// <summary>
        /// 新增地址
        /// </summary>
        /// <param name="addressBook"></param>
        /// <returns></returns>
        Task SaveAsync(AddressBookDTO addressBookDTO);
    }
}
