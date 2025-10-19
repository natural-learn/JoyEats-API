using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Common.Helpers;
using SkyTakeOut.Core.DTO.Employee;
using SkyTakeOut.Core.VO;
using SkyTakeOut.IRepository;
using SkyTakeOut.IRepository.UnitOfWork;
using SkyTakeOut.IServices;
using SkyTakeOut.Models;

namespace SkyTakeOut.Admin.Controllers
{
    /// <summary>
    /// 员工管理
    /// </summary>
    [Route("admin/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly JWTHelper _jWTHelper;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService, JWTHelper jWTHelper)
        {
            _logger = logger;
            _employeeService = employeeService;
            _jWTHelper = jWTHelper;
        }

        /// <summary>
        /// 员工登录
        /// </summary>
        /// <param name="employeeLoginDTO"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResult<EmployeeLoginVo>>> Login([FromBody] EmployeeLoginDTO employeeLoginDTO)
        {
            _logger.LogInformation("员工登录：{@EmployeeLoginDTO}", employeeLoginDTO);
            Employee? employee = await _employeeService.LoginAsync(employeeLoginDTO);

            // 生成Token
            string token = null;
            if (employee.Id == 1)
            {
                token = _jWTHelper.CreateToken(employee.Id.ToString(), "admin");
            }
            else
            {
                token = _jWTHelper.CreateToken(employee.Id.ToString(), "user");
            }
            EmployeeLoginVo employeeLoginVo = new EmployeeLoginVo
            {
                Id = employee.Id,
                Name = employee.Name,
                Username = employee.Username,
                Token = token
            };
            return ApiResultHelper.Success(employeeLoginVo);
        }
    }
}
