using Autofac;
using AutoMapper;
using JoyEats.IRepository.UnitOfWork;

namespace JoyEats.Services
{
    public abstract class BaseService
    {
        public ILifetimeScope LifetimeScope { get; set; }

        public IUnitOfWork UnitOfWork => LazyGetService<IUnitOfWork>();
        public IMapper Mapper => LazyGetService<IMapper>();

        public T LazyGetService<T>() where T : notnull
        {
            return new Lazy<T>(() => LifetimeScope.Resolve<T>()).Value;
        }

    }
}
