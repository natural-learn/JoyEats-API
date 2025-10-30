using Microsoft.EntityFrameworkCore;
using SkyTakeOut.Common;
using SkyTakeOut.Common.Constant;
using SkyTakeOut.Core.DTO.Setmeal;
using SkyTakeOut.Core.Exceptions;
using SkyTakeOut.Core.VO.Setmeal;
using SkyTakeOut.IRepository;
using SkyTakeOut.IRepository.UnitOfWork;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;

namespace SkyTakeOut.Services
{
    public class SetmealService : BaseService, ISetmealService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Setmeal> _setmealRepository;
        private readonly IBaseRepository<SetmealDish> _setmealDishRepository;
        private readonly IDishRepository _dishRepository;

        public SetmealService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _setmealRepository = unitOfWork.GetBaseRepository<Setmeal>();
            _setmealDishRepository = unitOfWork.GetBaseRepository<SetmealDish>();
            _dishRepository = unitOfWork.GetRepository<IDishRepository>();
        }

        /// <summary>
        /// 新增套餐
        /// </summary>
        /// <param name="setmealDTO"></param>
        /// <returns></returns>
        public async Task SaveWithDishAsync(SetmealDTO setmealDTO)
        {
            Setmeal setmeal = Mapper.Map<Setmeal>(setmealDTO);
            await _setmealRepository.AddAsync(setmeal);
            await _unitOfWork.SaveChangesAsync();
            Console.WriteLine($"现在的Setmeal的Id是{setmeal.Id}");

            List<SetmealDish> setmealDisheList = setmealDTO.SetmealDishes;
            foreach (SetmealDish setmealDish in setmealDisheList)
            {
                setmealDish.SetmealId = setmeal.Id;
            }

            await _setmealDishRepository.AddRangeAsync(setmealDisheList);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="setmealPageQueryDTO"></param>
        /// <returns></returns>
        public async Task<PagedResult<SetmealVo>> PageQueryAsync(SetmealPageQueryDTO setmealPageQueryDTO)
        {
            var setmealVo = await _setmealRepository.GetQueryable()
                .OrderByDescending(s => s.CreateTime)
                .Skip((setmealPageQueryDTO.Page - 1) * setmealPageQueryDTO.PageSize)
                .Take(setmealPageQueryDTO.PageSize)
                .Select(s => new SetmealVo
                {
                    Id = s.Id,
                    CategoryId = s.CategoryId,
                    Description = s.Description,
                    Name = s.Name,
                    CategoryName = s.Category != null ? s.Category.Name : null,
                    Image = s.Image,
                    Price = s.Price,
                    Status = s.Status.Value,
                    UpdateTime = s.UpdateTime.Value
                })
                .ToListAsync();
            var total = await _setmealRepository.GetQueryable().CountAsync();
            return PagedResult<SetmealVo>.GetPagedResult(setmealVo, setmealPageQueryDTO.Page, setmealPageQueryDTO.PageSize, total);
        }

        /// <summary>
        /// 套餐启售停售
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task StartOrStopAsync(int status, long id)
        {
            // 启售套餐时，判断套餐内是否有停售菜品，有停售菜品提示"套餐内包含未启售菜品，无法启售"
            if (status == StatusConstant.ENABLE)
            {
                List<Dish> dishList = await _dishRepository.GetBySetmealIdAsync(id);
                if (dishList != null && dishList.Count > 0)
                {
                    foreach (Dish dish in dishList)
                    {
                        if (dish.Status ==  StatusConstant.DISABLE)
                        {
                            throw new SetmealEnableFailedException(MessageConstant.SETMEAL_ENABLE_FAILED);
                        }
                    }
                }
            }

            Setmeal? setmeal = await _setmealRepository.GetByIdAsync(id) ?? 
                throw new EntityNotFoundException($"未找到id为{id}的套餐");

            setmeal.Status = status;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
