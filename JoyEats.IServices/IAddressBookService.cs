using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Core.DTO.AddressBook;
using JoyEats.Models;

namespace JoyEats.IServices
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
        /// <param name="addressBookDTO"></param>
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
        /// <param name="id"></param>
        /// <returns></returns>
        Task SetDefaultAsync(long id);

        /// <summary>
        /// 根据Id删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteByIdAsync(long id);

        /// <summary>
        /// 查询默认地址
        /// </summary>
        /// <returns></returns>
        Task<AddressBook?> GetDefaultAsync();
    }
}
