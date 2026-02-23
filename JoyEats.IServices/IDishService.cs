using JoyEats.Common;
using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Core.DTO.Dish;
using JoyEats.Core.VO.Dish;
using JoyEats.Models;

namespace JoyEats.IServices
{
    public interface IDishService : IScopeDependency
    {
        /// <summary>
        /// 新增菜品和对应的口味
        /// </summary>
        /// <param name="dishDTO"></param>
        /// <returns></returns>
        Task SaveWithFlavorAsync(DishDTO dishDTO);

        /// <summary>
        /// 菜品分页查询
        /// </summary>
        /// <param name="dishPageQueryDTO"></param>
        /// <returns></returns>
        Task<PagedResult<DishVo>> PageQueryAsync(DishPageQueryDTO dishPageQueryDTO);

        /// <summary>
        /// 菜品批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteBatchAsync(List<long> ids);

        /// <summary>
        /// 菜品启售停售
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task StartOrStopAsync(int status, long id);

        /// <summary>
        /// 根据id查询菜品和对应的口味数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DishVo> GetByIdWithFlavorAsync(long id);

        /// <summary>
        /// 根据id修改菜品基本信息和对应的口味信息
        /// </summary>
        /// <param name="dishDTO"></param>
        /// <returns></returns>
        Task UpdateWithFlavorAsync(DishDTO dishDTO);

        /// <summary>
        /// 根据分类id查询菜品
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<Dish>> ListAsync(long categoryId);

        /// <summary>
        /// 条件查询菜品和口味
        /// </summary>
        /// <param name="dish"></param>
        /// <returns></returns>
        Task<List<DishVo>> ListWithFlavorAsync(Dish dish);
    }
}
