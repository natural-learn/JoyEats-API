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

        /// <summary>
        /// 根据Id查询地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AddressBook?> GetByIdAsync(long id);

        /// <summary>
        /// 根据Id修改地址
        /// </summary>
        /// <param name="addressBookDTO"></param>
        /// <returns></returns>
        Task UpdateAsync(AddressBookDTO addressBookDTO);

        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="addressBook"></param>
        /// <returns></returns>
        Task SetDefaultAsync(AddressBook addressBook);
    }
}
