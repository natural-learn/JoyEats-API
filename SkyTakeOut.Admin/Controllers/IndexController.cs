using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.IRepository.UnitOfWork;
using SkyTakeOut.Models;

namespace SkyTakeOut.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly ILogger<IndexController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public IndexController(ILogger<IndexController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("hello")]
        public IActionResult Hello()
        {
            _logger.LogInformation($"测试日志记录功能成功!!!");
            return Ok("请求成功");
        }

        [HttpGet("insertData")]
        public async Task<IActionResult> InsertData()
        {
            var empRepository = _unitOfWork.GetBaseRepository<Employee>();
            await empRepository.AddAsync(new Employee
            {
                Id = 1,
                Name = "管理员",
                Username = "admin",
                Password = PasswordHelper.HashPassword("123456"),
                Phone = "13812312312",
                Sex = "1",
                IdNumber = "110101199001010047",
                Status = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                CreateUser = 10,
                UpdateUser = 1
            });
            await _unitOfWork.SaveChangesAsync();
            return Ok("数据添加成功");
        }

        [HttpGet("getEmpList")]
        public async Task<IActionResult> GetEmpList()
        {
            var empList = await _unitOfWork.GetBaseRepository<Employee>().GetListAsync();
            return Ok(empList);
        }
    }
}
