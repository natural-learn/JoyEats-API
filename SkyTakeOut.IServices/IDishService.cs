using SkyTakeOut.Common;
using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Core.DTO.Dish;
using SkyTakeOut.Core.VO.Dish;

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
    }
}
