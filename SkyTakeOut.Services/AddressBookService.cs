using Microsoft.EntityFrameworkCore;
using SkyTakeOut.Common.Constant;
using SkyTakeOut.Common.Helpers.Redis;
using SkyTakeOut.Core.DTO.AddressBook;
using SkyTakeOut.Core.Exceptions;
using SkyTakeOut.EntityFrameworkCore.Extensions;
using SkyTakeOut.IRepository;
using SkyTakeOut.IRepository.UnitOfWork;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;

namespace SkyTakeOut.Services
{
    public class AddressBookService : BaseService, IAddressBookService
    {
        private readonly long userId;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<AddressBook> _addressBookRepository;

        public AddressBookService(IUnitOfWork unitOfWork)
        {
            userId = long.Parse(StackExchangeRedisHelper.StringGetAsync(RedisConstant.UserId).Result);
            _unitOfWork = unitOfWork;
            _addressBookRepository = unitOfWork.GetBaseRepository<AddressBook>();
        }

        /// <summary>
        /// 查询当前登录用户的所有地址信息
        /// </summary>
        /// <param name="addressBook"></param>
        /// <returns></returns>
        public async Task<List<AddressBook>> ListAsync(AddressBook addressBook)
        {
            addressBook.UserId = userId;
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
        /// <param name="addressBook"></param>
        /// <returns></returns>
        public async Task SaveAsync(AddressBookDTO addressBookDTO)
        {
            addressBookDTO.UserId = userId;
            addressBookDTO.IsDefault = 0;
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
            _addressBookRepository.Update(updateEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
