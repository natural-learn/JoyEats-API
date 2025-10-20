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

        [HttpGet("insertCategoryData")]
        public async Task<IActionResult> InsertCategory()
        {
            var categoryRepository = _unitOfWork.GetBaseRepository<Category>();

            await categoryRepository.AddRangeAsync(new List<Category>
            {
                new Category() { Name = "酒水饮料", Sort = 10, Status = 1, Type = 1, CreateTime = DateTime.Now , UpdateTime = DateTime.Now, CreateUser = 1, UpdateUser = 1},
                new Category() { Name = "传统主食", Sort = 9, Status = 1, Type = 1, CreateTime = DateTime.Now , UpdateTime = DateTime.Now, CreateUser = 1, UpdateUser = 1},
                new Category() { Name = "人气套餐", Sort = 12, Status = 1, Type = 2, CreateTime = DateTime.Now , UpdateTime = DateTime.Now, CreateUser = 1, UpdateUser = 1},
                new Category() { Name = "商务套餐", Sort = 13, Status = 1, Type = 2, CreateTime = DateTime.Now , UpdateTime = DateTime.Now, CreateUser = 1, UpdateUser = 1},
                new Category() { Name = "蜀味烤鱼", Sort = 4, Status = 1, Type = 1, CreateTime = DateTime.Now , UpdateTime = DateTime.Now, CreateUser = 1, UpdateUser = 1},
                new Category() { Name = "蜀味牛蛙", Sort = 5, Status = 1, Type = 1, CreateTime = DateTime.Now , UpdateTime = DateTime.Now, CreateUser = 1, UpdateUser = 1},
                new Category() { Name = "特色蒸菜", Sort = 6, Status = 1, Type = 1, CreateTime = DateTime.Now , UpdateTime = DateTime.Now, CreateUser = 1, UpdateUser = 1},
                new Category() { Name = "新鲜时蔬", Sort = 7, Status = 1, Type = 1, CreateTime = DateTime.Now , UpdateTime = DateTime.Now, CreateUser = 1, UpdateUser = 1},
                new Category() { Name = "水煮鱼", Sort = 8, Status = 1, Type = 1, CreateTime = DateTime.Now , UpdateTime = DateTime.Now, CreateUser = 1, UpdateUser = 1},
                new Category() { Name = "汤类", Sort = 11, Status = 1, Type = 1, CreateTime = DateTime.Now , UpdateTime = DateTime.Now, CreateUser = 1, UpdateUser = 1}
            });
            await _unitOfWork.SaveChangesAsync();
            return Ok("分类数据添加成功");
        }

        [HttpGet("getEmpList")]
        public async Task<IActionResult> GetEmpList()
        {
            var empList = await _unitOfWork.GetBaseRepository<Employee>().GetListAsync();
            return Ok(empList);
        }


    }
}
