using SkyTakeOut.Common;
using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Core.DTO.Category;
using SkyTakeOut.Models;

namespace SkyTakeOut.IServices
{
    public interface ICategoryService : IScopeDependency
    {
        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        Task SaveAsync(CategoryDTO categoryDTO);

        /// <summary>
        /// 分类分页查询
        /// </summary>
        /// <param name="categoryPageQueryDTO"></param>
        /// <returns></returns>
        Task<PagedResult<Category>> PageQueryAsync(CategoryPageQueryDTO categoryPageQueryDTO);

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteByCategoryIdAsync(long id);

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        Task UpdateCategoryAsync(CategoryDTO categoryDTO);

        /// <summary>
        /// 启用、禁用分类
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task StartOrStopAsync(int status, long id);

        /// <summary>
        /// 根据类型查询分类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<List<Category>> ListAsync(int type);
    }
}
