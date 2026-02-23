using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Models;

namespace JoyEats.IRepository
{
    public interface IOrderDetailRepository : IBaseRepository<OrderDetail>, IScopeDependency
    {
    }
}
