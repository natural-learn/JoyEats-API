using Microsoft.EntityFrameworkCore;
using SkyTakeOut.Common;
using SkyTakeOut.Core.DTO.Dish;
using SkyTakeOut.Core.VO.Dish;
using SkyTakeOut.IRepository;
using SkyTakeOut.IRepository.UnitOfWork;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;

namespace SkyTakeOut.Services
{
    public class DishService : BaseService, IDishService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Dish> _dishRepository;
        private readonly IBaseRepository<DishFlavor> _dishFlavorRepository;
        private readonly IBaseRepository<Category> _categoryRepository;

        public DishService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dishRepository = unitOfWork.GetBaseRepository<Dish>();
            _dishFlavorRepository = unitOfWork.GetBaseRepository<DishFlavor>();
            _categoryRepository = unitOfWork.GetBaseRepository<Category>();
        }

        /// <summary>
        /// 新增菜品和对应的口味
        /// </summary>
        /// <param name="dishDTO"></param>
        /// <returns></returns>
        public async Task SaveWithFlavorAsync(DishDTO dishDTO)
        {
            Dish? dish = Mapper.Map<Dish>(dishDTO);
            await _dishRepository.AddAsync(dish);

            // 保存菜品口味数据
            List<DishFlavor> dishFlavors = dishDTO.Flavors;
            if (dishFlavors != null && dishFlavors.Count > 0)
            {
                foreach (var flavor in dishFlavors)
                {
                    flavor.DishId = dish.Id;
                }
                await _dishFlavorRepository.AddRangeAsync(dishFlavors);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 菜品分页查询
        /// </summary>
        /// <param name="dishPageQueryDTO"></param>
        /// <returns></returns>
        public async Task<PagedResult<DishVo>> PageQueryAsync(DishPageQueryDTO dishPageQueryDTO)
        {
            var dishVoList = await _dishRepository.GetQueryable()
                .Select(d => new DishVo
                {
                    Id = d.Id,
                    Name = d.Name,
                    CategoryId = d.Category.Id,
                    CategoryName = d.Category != null ? d.Category.Name : null,
                    Price = d.Price,
                    Image = d.Image,
                    Description = d.Description,
                    Status = d.Status,
                    UpdateTime = d.UpdateTime
                })
                .ToListAsync();

            return PagedResult<DishVo>.GetPagedResult(dishVoList, dishPageQueryDTO.Page, dishPageQueryDTO.PageSize, dishVoList.Count);
        }
    }
}
