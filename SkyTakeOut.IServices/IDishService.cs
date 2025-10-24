using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Core.DTO.Dish;

namespace SkyTakeOut.IServices
{
    public interface IDishService : IScopeDependency
    {
        /// <summary>
        /// 新增菜品和对应的口味
        /// </summary>
        /// <param name="dishDTO"></param>
        /// <returns></returns>
        Task SaveWithFlavorAsync(DishDTO dishDTO);


    }
}
