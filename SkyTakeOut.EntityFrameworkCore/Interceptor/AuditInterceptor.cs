using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SkyTakeOut.Common.Constant;
using SkyTakeOut.Common.Helpers.Redis;
using SkyTakeOut.Models;

namespace SkyTakeOut.EntityFrameworkCore.Interceptor
{
    public class AuditInterceptor : SaveChangesInterceptor
    {

        public AuditInterceptor()
        {
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await HandleAuditProperties(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task HandleAuditProperties(DbContext? context)
        {
            if (context == null)
            {
                return;
            }

            try
            {
                var employeeId = long.Parse((await StackExchangeRedisHelper.StringGetAsync(RedisConstant.EmployeeId)));
                Console.WriteLine($"获取到employeeId：{employeeId}");
                Console.WriteLine($"Redis获取到employeeId：{await StackExchangeRedisHelper.StringGetAsync(RedisConstant.EmployeeId)}");
                
                foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
                {
                    switch (entry.State)
                    {
                        // 新增实体：设置创建时间、创建人、修改时间、修改人
                        case EntityState.Added:
                            entry.Entity.CreateTime = DateTime.Now;
                            entry.Entity.CreateUser = employeeId;
                            entry.Entity.UpdateTime = DateTime.Now;
                            entry.Entity.UpdateUser = employeeId;
                            break;
                        // 修改实体：设置修改时间、修改人（不覆盖创建信息）
                        case EntityState.Modified:
                            entry.Entity.UpdateTime = DateTime.Now;
                            entry.Entity.UpdateUser = employeeId;
                            // 可选：禁止修改创建时间和创建人（防止手动篡改）
                            entry.Property(e => e.CreateTime).IsModified = false;
                            entry.Property(e => e.CreateUser).IsModified = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
