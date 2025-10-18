using SkyTakeOut.Common;
using System.Linq.Expressions;

namespace SkyTakeOut.IRepository
{
    /// <summary>
    /// 空接口，用于类型约束
    /// </summary>
    public interface IBaseRepository
    {

    }

    public interface IBaseRepository<TEntity> : IBaseRepository where TEntity : class
    {
        #region 新增
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">要插入的实体</param>
        /// <param name="cancellationToken">取消异步操作</param>
        /// <returns></returns>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities">要插入的数据集合</param>
        /// <param name="cancellationToken">取消异步操作</param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        #endregion

        #region 删除
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="cancellationToken">取消异步操作</param>
        /// <returns></returns>
        void DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        void DeleteRangeAsync(IEnumerable<TEntity> entities);
        #endregion

        #region 更新
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        void Update(TEntity entity);
        #endregion

        #region 查询
        /// <summary>
        /// 根据主键查询数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件返回符合条件的第一个数据，如果没有则返回null
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// 返回符合条件的所有数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// 返回符合条件的所有数据并排序
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAscending"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate,
                                         Expression<Func<TEntity, object>>? orderBy,
                                         bool isAscending = true,
                                         CancellationToken cancellationToken = default);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="isAscending">是否升序</param>
        /// <param name="cancellationToken">取消异步操作</param>
        /// <returns></returns>
        Task<PagedResult<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>>? predicate,
                                                     Expression<Func<TEntity, object>>? orderBy,
                                                     int pageIndex = 1,
                                                     int pageSize = 10,
                                                     bool isAscending = true,
                                                     CancellationToken cancellationToken = default);

        /// <summary>
        /// 返回符合条件的数量
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        #endregion

        /// <summary>
        /// 执行原生SQL语句，返回受影响的行数。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters);

    }
}
