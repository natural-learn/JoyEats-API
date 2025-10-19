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
    }
}
