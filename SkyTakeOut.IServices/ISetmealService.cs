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
    }
}
