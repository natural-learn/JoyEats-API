using SkyTakeOut.Common;
using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Core.DTO.Setmeal;
using SkyTakeOut.Core.VO.Setmeal;

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
    }
}
