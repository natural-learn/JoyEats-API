using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Core.DTO.Category;

namespace SkyTakeOut.IServices
{
    public interface ICategoryService : IScopeDependency
    {
        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task SaveAsync(CategoryDTO categoryDTO);
    }
}
