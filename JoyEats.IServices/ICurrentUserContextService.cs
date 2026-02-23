using JoyEats.Core.Autofac.DependencyInjection;

namespace JoyEats.IServices
{
    public interface ICurrentUserContextService : ISingletonDependency
    {
        /// <summary>
        /// 获取当前用户Id
        /// </summary>
        /// <returns></returns>
        Task<long> GetCurrentUserIdAsync();
    }
}
