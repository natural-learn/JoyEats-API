using JoyEats.EntityFrameworkCore;
using JoyEats.IRepository;
using JoyEats.Models;

namespace SkyTakeOut.Repository
{
    public class OrderDetailRepository : EFCoreRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(AppDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
