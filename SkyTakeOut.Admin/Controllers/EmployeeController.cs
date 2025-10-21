using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Common.Helpers;
using SkyTakeOut.Core.DTO.Employee;
using SkyTakeOut.Core.VO;
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
                token = _jWTHelper.CreateToken(employee.Id.ToString(), employee.Username, "admin");
            }
            else
            {
                token = _jWTHelper.CreateToken(employee.Id.ToString(), employee.Username, "user");
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

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public ActionResult<ApiResult> Logout()
        {
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="employeeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResult>> Save([FromBody] EmployeeDTO employeeDTO)
        {
            _logger.LogInformation("新增员工：{@EmployeeDTO}", employeeDTO);
            await _employeeService.SaveAsync(employeeDTO);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 分页查询所有员工
        /// </summary>
        /// <param name="employeePageQueryDTO"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<ActionResult<ApiResult<PagedResult<Employee>>>> Page([FromQuery] EmployeePageQueryDTO employeePageQueryDTO)
        {
            _logger.LogInformation("员工分页查询：{@EmployeePageQueryDTO}", employeePageQueryDTO);
            var pagedList = await _employeeService.PageQueryAsync(employeePageQueryDTO);
            return ApiResultHelper.Success(pagedList);
        }

        /// <summary>
        /// 启用禁用员工账号
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("status/{status}")]
        public async Task<ActionResult<ApiResult>> StartOrStop(int status, long id)
        {
            _logger.LogInformation("启用禁用员工账号：{@int},{@long}", status, id);
            await _employeeService.StartOrStopAsync(status, id);
            return ApiResultHelper.Success();
        }

        /// <summary>
        /// 根据Id查询员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<Employee>>> GetById(long id)
        {
            Employee? employee = await _employeeService.GetByIdAsync(id);
            if (employee != null)
            {
                employee.Password = "******";
                return ApiResultHelper.Success(employee);
            }
            return ApiResultHelper.Error<Employee>("员工不存在");
        }

        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <param name="employeeDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResult>> Update([FromBody] EmployeeDTO employeeDTO)
        {
            _logger.LogInformation("编辑员工信息：{@EmployeeDTO}", employeeDTO);
            await _employeeService.UpdateEmployeeAsync(employeeDTO);
            return ApiResultHelper.Success();
        }
    }
}
