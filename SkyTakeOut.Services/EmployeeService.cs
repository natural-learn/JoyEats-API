using SkyTakeOut.Common;
using SkyTakeOut.Common.Constant;
using SkyTakeOut.Core.DTO.Employee;
using SkyTakeOut.Core.Exceptions;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;

namespace SkyTakeOut.Services
{
    public class EmployeeService : BaseService, IEmployeeService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="employeeLoginDTO"></param>
        /// <returns></returns>
        public async Task<Employee> LoginAsync(EmployeeLoginDTO employeeLoginDTO)
        {
            var userName = employeeLoginDTO.UserName;
            var password = employeeLoginDTO.Password;

            var employeeRepository = UnitOfWork.GetBaseRepository<Employee>();
            Employee? employee = await employeeRepository.FirstOrDefaultAsync(emp => emp.Username == userName) ??
                throw new AccountNotFoundException(MessageConstant.ACCOUNT_NOT_FOUND);
            if (!PasswordHelper.VerifyPassword(password, employee.Password))
            {
                throw new PasswordErrorException(MessageConstant.PASSWORD_ERROR);
            }
            if(employee.Status == StatusConstant.DISABLE)
            {
                // 账号被锁定
                throw new AccountLockedException(MessageConstant.ACCOUNT_LOCKED);
            }
            return employee;
        }

    }
}
