using JoyEats.Common;
using JoyEats.Core.Autofac.DependencyInjection;
using JoyEats.Core.DTO.Employee;
using JoyEats.Models;

namespace JoyEats.IServices
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

        /// <summary>
        /// 根据id查询员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Employee?> GetByIdAsync(long id);

        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <param name="employeeDTO"></param>
        /// <returns></returns>
        Task UpdateEmployeeAsync(EmployeeDTO employeeDTO);
    }
}
