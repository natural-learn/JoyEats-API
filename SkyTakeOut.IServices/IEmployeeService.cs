using SkyTakeOut.Common;
using SkyTakeOut.Core.Autofac.DependencyInjection;
using SkyTakeOut.Core.DTO.Employee;
using SkyTakeOut.Models;

namespace SkyTakeOut.IServices
{
    public interface IEmployeeService : IScopeDependency
    {
        /// <summary>
        /// 员工登录
        /// </summary>
        /// <param name="employeeLoginDTO"></param>
        /// <returns></returns>
        Task<Employee> LoginAsync(EmployeeLoginDTO employeeLoginDTO);

        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="employeeDTO"></param>
        /// <returns></returns>
        Task SaveAsync(EmployeeDTO employeeDTO);

        /// <summary>
        /// 分页查询所有员工
        /// </summary>
        /// <param name="employeePageQueryDTO"></param>
        /// <returns></returns>
        Task<PagedResult<Employee>> PageQueryAsync(EmployeePageQueryDTO employeePageQueryDTO);

        /// <summary>
        /// 启用禁用员工账号
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task StartOrStopAsync(int status, long id);
    }
}
