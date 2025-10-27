using Microsoft.EntityFrameworkCore;
using SkyTakeOut.Common;
using SkyTakeOut.Common.Constant;
using SkyTakeOut.Core.DTO.Dish;
using SkyTakeOut.Core.Exceptions;
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
        private readonly IBaseRepository<SetmealDish> _setmealDishRepository;

        public DishService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dishRepository = unitOfWork.GetBaseRepository<Dish>();
            _dishFlavorRepository = unitOfWork.GetBaseRepository<DishFlavor>();
            _categoryRepository = unitOfWork.GetBaseRepository<Category>();
            _setmealDishRepository = unitOfWork.GetBaseRepository<SetmealDish>();
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

        /// <summary>
        /// 菜品批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteBatchAsync(List<long> ids)
        {
            // 去重处理
            var distinctIds = ids.Distinct().ToList();
            if (distinctIds.Count == 0)
            {
                return;
            }

            // 批量查询所有Id对应的菜品（仅一次数据库查询）
            List<Dish> dishList = await _dishRepository
                .GetQueryable()
                .Where(d => distinctIds.Contains(d.Id))
                .ToListAsync();

            // 找出不存在的Id
            var existingIds = dishList.Select(d => d.Id).ToList();
            var nonExistingIds = distinctIds.Except(existingIds).ToList();
            if (nonExistingIds.Count != 0)
            {
                throw new EntityNotFoundException(
                    $"找不到以下id的菜品：{string.Join(",", nonExistingIds)}");
            }

            // 如果菜品处于启售状态，则不能删除
            if (dishList.Any(d => d.Status == StatusConstant.ENABLE))
            {
                throw new DeletionNotAllowedException(MessageConstant.DISH_BE_RELATED_BY_SETMEAL);
            }

            // 如果菜品被套餐关联，则不能删除
            List<long?> setmealIds = await _setmealDishRepository
                .GetQueryable()
                .AsNoTracking()
                .Where(sd => sd.DishId.HasValue && distinctIds.Contains(sd.DishId.Value))
                .Select(sd => sd.SetmealId)
                .Distinct()
                .ToListAsync();

            if (setmealIds != null && setmealIds.Count > 0)
            {
                throw new DeletionNotAllowedException(MessageConstant.DISH_BE_RELATED_BY_SETMEAL);
            }

            // 批量删除
            string parameters = string.Join(",", distinctIds.Select((_, index) => $"@p{index}"));
            string sql = $"DELETE FROM Dish WHERE Id IN ({parameters})";
            await _dishRepository.ExecuteSqlAsync(sql, parameters: distinctIds.Cast<object>().ToArray());

            sql = $"DELETE FROM DishFlavor WHERE DishId IN ({parameters})";
            await _dishFlavorRepository.ExecuteSqlAsync(sql, parameters: distinctIds.Cast<object>().ToArray());
        }


        /// <summary>
        /// 菜品启售停售
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task StartOrStopAsync(int status, long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("菜品id必须为正整数");
            }

            Dish dish = await _dishRepository.GetByIdAsync(id) ??
                throw new EntityNotFoundException($"未找到id为{id}的菜品");

            if (dish.Status == status)
            {
                return;
            }

            dish.Status = status;
            _dishRepository.Update(dish);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
