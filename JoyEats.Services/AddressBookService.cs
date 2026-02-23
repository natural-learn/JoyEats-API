using JoyEats.Core.DTO.AddressBook;
using JoyEats.Core.Exceptions;
using JoyEats.EntityFrameworkCore.Extensions;
using JoyEats.IRepository;
using JoyEats.IRepository.UnitOfWork;
using JoyEats.IServices;
using JoyEats.Models;
using Microsoft.EntityFrameworkCore;

namespace JoyEats.Services
{
    public class AddressBookService : BaseService, IAddressBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<AddressBook> _addressBookRepository;
        private readonly ICurrentUserContextService _currentUserContextService;

        public AddressBookService(IUnitOfWork unitOfWork, ICurrentUserContextService currentUserContextService)
        {
            _unitOfWork = unitOfWork;
            _addressBookRepository = unitOfWork.GetBaseRepository<AddressBook>();
            _currentUserContextService = currentUserContextService;
        }

        /// <summary>
        /// 查询当前登录用户的所有地址信息
        /// </summary>
        /// <param name="addressBook"></param>
        /// <returns></returns>
        public async Task<List<AddressBook>> ListAsync(AddressBook addressBook)
        {
            addressBook.UserId = await _currentUserContextService.GetCurrentUserIdAsync();
            // 要判断IsDefault是否为空才行
            return await _addressBookRepository
                .GetQueryable()
                .WhereIF(addressBook.UserId != 0, a => a.UserId == addressBook.UserId)
                .WhereIF(!string.IsNullOrWhiteSpace(addressBook.Phone), a => a.Phone == addressBook.Phone)
                .ToListAsync();
        }

        /// <summary>
        /// 新增地址
        /// </summary>
        /// <param name="addressBookDTO"></param>
        /// <returns></returns>
        public async Task SaveAsync(AddressBookDTO addressBookDTO)
        {
            addressBookDTO.UserId = await _currentUserContextService.GetCurrentUserIdAsync();
            AddressBook addressBook = Mapper.Map<AddressBook>(addressBookDTO);
            await _addressBookRepository.AddAsync(addressBook);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据Id查询地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AddressBook?> GetByIdAsync(long id)
        {
            return await _addressBookRepository.GetByIdAsync(id);
        }


        /// <summary>
        /// 根据Id修改地址
        /// </summary>
        /// <param name="addressBookDTO"></param>
        /// <returns></returns>
        public async Task UpdateAsync(AddressBookDTO addressBookDTO)
        {
            var updateEntity = await _addressBookRepository.GetByIdAsync(addressBookDTO.Id) ??
                throw new EntityNotFoundException($"未找到Id为{addressBookDTO.Id}的地址");
            Mapper.Map(addressBookDTO, updateEntity);
            updateEntity.UserId = await _currentUserContextService.GetCurrentUserIdAsync();
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SetDefaultAsync(long id)
        {
            var addressBook = await _addressBookRepository.GetByIdAsync(id) ??
                throw new EntityNotFoundException($"未找到Id为{id}的地址");
            var userId = await _currentUserContextService.GetCurrentUserIdAsync();
            // 将当前用户的所有地址修改为非默认地址
            var userAddressList = await _addressBookRepository
                .GetListAsync(a => a.UserId == userId);

            foreach (var userAddress in userAddressList)
            {
                addressBook.IsDefault = 0;
                _addressBookRepository.Update(userAddress);
            }

            // 将当前地址改为默认地址
            addressBook.IsDefault = 1;
            addressBook.UserId = userId;
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据Id删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteByIdAsync(long id)
        {
            await _addressBookRepository.RemoveByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 查询默认地址
        /// </summary>
        /// <returns></returns>
        public async Task<AddressBook?> GetDefaultAsync()
        {
            var userId = await _currentUserContextService.GetCurrentUserIdAsync();
            return await _addressBookRepository.SingleOrDefaultAsync(a => a.UserId == userId && a.IsDefault == 1);
        }
    }
}
