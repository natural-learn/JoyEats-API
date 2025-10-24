using SkyTakeOut.Core.DTO.Dish;
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

        public DishService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dishRepository = unitOfWork.GetBaseRepository<Dish>();
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
        }
    }
}
