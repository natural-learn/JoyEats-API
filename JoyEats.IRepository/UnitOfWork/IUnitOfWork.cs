using JoyEats.Core.Autofac.DependencyInjection;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace JoyEats.IRepository.UnitOfWork
{
    public interface IUnitOfWork : IScopeDependency, IDisposable
    {
        /// <summary>
        /// 获取通用仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IBaseRepository<TEntity> GetBaseRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// 获取具体仓储
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <returns></returns>
        TRepository GetRepository<TRepository>() where TRepository : IBaseRepository;

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /**
         * EFCore的DbContext在调用SaveChanges或SaveChangesAsync方法时会自动开启和提交事务，并在以下情况自动回滚：
         * 1. 执行过程中抛出异常（如数据库约束冲突、网络错误等）
         * 2. 未调用 SaveChangesAsync（所有内存中的修改不会提交到数据库）
         */

        /// <summary>
        /// 手动开启事务
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IDbContextTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 提交手动开启的事务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 回滚手动开启的事务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
