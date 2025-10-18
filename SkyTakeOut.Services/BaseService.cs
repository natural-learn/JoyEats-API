using Autofac;
using AutoMapper;
using SkyTakeOut.IRepository.UnitOfWork;

namespace SkyTakeOut.Services
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
