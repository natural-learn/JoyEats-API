using Autofac;
using JoyEats.EntityFrameworkCore;
using JoyEats.IRepository;
using JoyEats.IRepository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace JoyEats.Repository.UnitOfWork
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IComponentContext _componentContext;
        private readonly Dictionary<Type, object> _repositories = new();
        private IDbContextTransaction _currentTransaction;  //跟踪当前事务

        public EfCoreUnitOfWork(AppDbContext dbContext, IComponentContext componentContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _componentContext = componentContext;
        }

        /// <summary>
        /// 获取通用仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IBaseRepository<TEntity> GetBaseRepository<TEntity>() where TEntity : class
        {
            var entityType = typeof(TEntity);

            // 检查缓存：若字典中已有该类型的仓储，直接复用
            if (_repositories.TryGetValue(entityType, out var repository))
            {
                return (IBaseRepository<TEntity>)repository;
            }

            var newRepository = new EFCoreRepository<TEntity>(_dbContext);
            _repositories[entityType] = newRepository;
            return newRepository;
        }

        /// <summary>
        /// 获取具体仓储
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <returns></returns>
        public TIRepository GetRepository<TIRepository>() where TIRepository : IBaseRepository
        {
            // 避免重复创建
            if (_repositories.TryGetValue(typeof(TIRepository), out var repo))
            {
                return (TIRepository)repo;
            }

            // 从DI容器获取具体仓储实例（要确保与当前DbContext关联）
            var repository = _componentContext.Resolve<TIRepository>();
            _repositories[typeof(TIRepository)] = repository;
            return repository;
        }

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

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
        public async Task<IDbContextTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            // 如果已有事务，先释放
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
            }

            // 开启新事务并记录
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return _currentTransaction;
        }

        /// <summary>
        /// 提交手动开启的事务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("未开启事务，无法提交");
            }
            try
            {
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        /// <summary>
        /// 回滚手动开启的事务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("未开启事务，无法回滚");
            }
            try
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
