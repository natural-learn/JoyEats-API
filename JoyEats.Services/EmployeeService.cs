using JoyEats.Common;
using JoyEats.Common.Constant;
using JoyEats.Core.DTO.Employee;
using JoyEats.Core.Exceptions;
using JoyEats.IRepository;
using JoyEats.IRepository.UnitOfWork;
using JoyEats.IServices;
using JoyEats.Models;

namespace JoyEats.Services
{
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Employee> _employeeRepository;


        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _employeeRepository = unitOfWork.GetBaseRepository<Employee>();
            _unitOfWork = unitOfWork;
        }

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
            if (employee.Status == StatusConstant.DISABLE)
            {
                // 账号被锁定
                throw new AccountLockedException(MessageConstant.ACCOUNT_LOCKED);
            }
            return employee;
        }

        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="employeeDTO"></param>
        /// <returns></returns>
        public async Task SaveAsync(EmployeeDTO employeeDTO)
        {
            Employee employee = Mapper.Map<Employee>(employeeDTO);

            //设置账号状态，默认正常 1表示正常 0表示锁定
            employee.Status = StatusConstant.ENABLE;

            //设置密码，默认密码123456
            employee.Password = PasswordHelper.HashPassword(PasswordConstant.DEFAULT_PASSWORD);

            //设置当前记录的创建时间和修改时间
            employee.CreateTime = DateTime.Now;
            employee.UpdateTime = DateTime.Now;

            await _employeeRepository.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 分页查询所有员工
        /// </summary>
        /// <param name="employeePageQueryDTO"></param>
        /// <returns></returns>
        public async Task<PagedResult<Employee>> PageQueryAsync(EmployeePageQueryDTO employeePageQueryDTO)
        {
            string name = employeePageQueryDTO.Name?.Trim() ?? "";
            return await _employeeRepository.GetPagedListAsync(
                predicate: string.IsNullOrEmpty(name) ? null : e => e.Name.Contains(name),
                orderBy: e => e.CreateTime,
                isAscending: true,
                pageIndex: employeePageQueryDTO.Page,
                pageSize: employeePageQueryDTO.PageSize
            );
        }

        /// <summary>
        /// 启用禁用员工账号
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task StartOrStopAsync(int status, long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("员工id必须为正整数");
            }

            var employee = await _employeeRepository.GetByIdAsync(id) ?? 
                throw new EntityNotFoundException($"未找到id为{id}的员工");
            
            if (employee.Status == status)
            {
                return;
            }

            employee.Status = status;
            _employeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据id查询员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Employee?> GetByIdAsync(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("员工id必须为正整数");
            }

            return _employeeRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <param name="employeeDTO"></param>
        /// <returns></returns>
        public async Task UpdateEmployeeAsync(EmployeeDTO employeeDTO)
        {
            var employee = await GetByIdAsync(employeeDTO.Id) ??
                throw new EntityNotFoundException($"未找到id为{employeeDTO.Id}的员工"); ;

            var newEmployee = Mapper.Map(employeeDTO, employee);
            _employeeRepository.Update(newEmployee);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
