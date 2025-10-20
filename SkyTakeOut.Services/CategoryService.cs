using SkyTakeOut.Common;
using SkyTakeOut.Common.Constant;
using SkyTakeOut.Core.DTO.Category;
using SkyTakeOut.Core.Exceptions;
using SkyTakeOut.IRepository;
using SkyTakeOut.IRepository.UnitOfWork;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;
using System;
using System.Linq.Expressions;

namespace SkyTakeOut.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IBaseRepository<Dish> _dishRepository;
        private readonly IBaseRepository<Setmeal> _setmealRepository;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = unitOfWork.GetBaseRepository<Category>();
            _dishRepository = unitOfWork.GetBaseRepository<Dish>();
            _setmealRepository = unitOfWork.GetBaseRepository<Setmeal>();
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

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteByCategoryIdAsync(long id)
        {
            // 如果当前分类下有菜品，不能删除
            bool exists = await _dishRepository.ExistsAsync(d => d.CategoryId == id);
            if (exists)
            {
                throw new DeletionNotAllowedException(MessageConstant.CATEGORY_BE_RELATED_BY_DISH);
            }

            // 如果当前分类下有套餐，不能删除
            exists = await _setmealRepository.ExistsAsync(s => s.CategoryId == id);
            if (exists)
            {
                throw new DeletionNotAllowedException(MessageConstant.CATEGORY_BE_RELATED_BY_SETMEAL);
            }

            await _categoryRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        public async Task UpdateCategoryAsync(CategoryDTO categoryDTO)
        {
            Category? category = await _categoryRepository.GetByIdAsync(categoryDTO.Id) ??
                throw new EntityNotFoundException($"未找到Id为{categoryDTO.Id}的分类");
            category = Mapper.Map(categoryDTO, category);
            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 启用、禁用分类
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task StartOrStopAsync(int status, long id)
        {
            Category? category = await _categoryRepository.GetByIdAsync(id) ??
                throw new EntityNotFoundException($"未找到Id为{id}的分类");
            category.Status = status;
            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据类型查询分类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<Category>> ListAsync(int type)
        {
            if (type == 0)
            {
                return await _categoryRepository.GetListAsync(
                    predicate: c => c.Status == StatusConstant.ENABLE,
                    orderBy: c => c.Sort);
            }
            return await _categoryRepository.GetListAsync(
                    predicate: c => c.Status == StatusConstant.ENABLE && c.Type == type,
                    orderBy: c => c.Sort);
        }
    }
}
