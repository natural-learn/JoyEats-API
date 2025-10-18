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
    }
}
