using SkyTakeOut.Common;
using SkyTakeOut.Common.Constant;
using SkyTakeOut.Core.DTO.Category;
using SkyTakeOut.IRepository;
using SkyTakeOut.IRepository.UnitOfWork;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;
using System.Linq.Expressions;

namespace SkyTakeOut.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Category> _categoryRepository;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = unitOfWork.GetBaseRepository<Category>();
        }

        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        public async Task SaveAsync(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                throw new ArgumentNullException(nameof(categoryDTO), "分类信息不能为空");
            }

            if (string.IsNullOrWhiteSpace(categoryDTO.Name))
            {
                throw new ArgumentException("分类名称不能为空", nameof(categoryDTO.Name));
            }

            bool exists = await _categoryRepository.ExistsAsync(c => c.Name == categoryDTO.Name);
            if (exists)
            {
                throw new InvalidOperationException($"分类名称 '{categoryDTO.Name}' 已存在");
            }

            Category category = Mapper.Map<Category>(categoryDTO);
            category.Status = StatusConstant.ENABLE;
            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 分类分页查询
        /// </summary>
        /// <param name="categoryPageQueryDTO"></param>
        /// <returns></returns>
        public async Task<PagedResult<Category>> PageQueryAsync(CategoryPageQueryDTO categoryPageQueryDTO)
        {
            string name = categoryPageQueryDTO.Name?.Trim() ?? "";
            Expression<Func<Category, bool>>? predicate = c => true;
            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate.And(c => c.Name.Contains(name));
            }
            if (categoryPageQueryDTO.Type.HasValue)
            {
                predicate = predicate.And(c => c.Type == categoryPageQueryDTO.Type.Value);
            }

            return await _categoryRepository.GetPagedListAsync(
                predicate: predicate,
                orderBy: c => c.Sort,
                pageIndex: categoryPageQueryDTO.Page,
                pageSize: categoryPageQueryDTO.PageSize);
        }
    }
}
