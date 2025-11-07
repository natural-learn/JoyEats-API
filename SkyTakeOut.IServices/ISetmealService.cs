using SkyTakeOut.Common;
using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Core.DTO.Setmeal;
using SkyTakeOut.Core.VO.Dish;
using SkyTakeOut.Core.VO.Setmeal;
using SkyTakeOut.Models;

namespace SkyTakeOut.IServices
{
    public interface ISetmealService : IScopeDependency
    {
        /// <summary>
        /// 新增套餐
        /// </summary>
        /// <param name="setmealDTO"></param>
        /// <returns></returns>
        Task SaveWithDishAsync(SetmealDTO setmealDTO);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="setmealPageQueryDTO"></param>
        /// <returns></returns>
        Task<PagedResult<SetmealVo>> PageQueryAsync(SetmealPageQueryDTO setmealPageQueryDTO);

        /// <summary>
        /// 套餐启售停售
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task StartOrStopAsync(int status, long id);

        /// <summary>
        /// 根据id查询套餐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SetmealVo> GetByIdWithDishAsync(long id);

        /// <summary>
        /// 修改套餐
        /// </summary>
        /// <param name="setmealDTO"></param>
        /// <returns></returns>
        Task UpdateSetmealAsync(SetmealDTO setmealDTO);

        /// <summary>
        /// 批量删除套餐
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteBatchAsync(List<long> ids);

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="setmealDTO"></param>
        /// <returns></returns>
        Task<List<Setmeal>> ListAsync(SetmealDTO setmealDTO);

        /// <summary>
        /// 根据id查询菜品选项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<DishItemVo>> GetDishItemByIdAsync(long id);
    }
}
