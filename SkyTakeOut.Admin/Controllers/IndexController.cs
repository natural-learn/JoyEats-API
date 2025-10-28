using Microsoft.AspNetCore.Mvc;
using SkyTakeOut.Common;
using SkyTakeOut.Common.Constant;
using SkyTakeOut.Common.Helpers.Redis;
using SkyTakeOut.IRepository.UnitOfWork;
using SkyTakeOut.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SkyTakeOut.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly ILogger<IndexController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexController(ILogger<IndexController> logger, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("hello")]
        public async Task<IActionResult> Hello()
        {
            return Ok($"请求成功，获取到当前登录用户Id：{await StackExchangeRedisHelper.StringGetAsync(RedisConstant.EmployeeId)}");
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

        [HttpGet("insertDishData")]
        public async Task<IActionResult> InsertDishData()
        {
            List<Dish> dishList = new List<Dish>
            {
                new Dish{ Name = "王老吉",     CategoryId = 1, Price = 6.00m,  Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/41bfcacf-7ad4-4927-8b26-df366553a94c.png", Description = "", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "北冰洋",     CategoryId = 1, Price = 4.00m,  Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/4451d4be-89a2-4939-9c69-3a87151cb979.png", Description = "还是小时候的味道", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "雪花啤酒",   CategoryId = 1, Price = 4.00m,  Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/bf8cbfc1-04d2-40e8-9826-061ee41ab87c.png", Description = "", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "米饭",       CategoryId = 2, Price = 2.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/76752350-2121-44d2-b477-10791c23a8ec.png", Description = "精选五常大米", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "馒头",       CategoryId = 2, Price = 1.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/475cc599-8661-4899-8f9e-121dd8ef7d02.png", Description = "优质面粉", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "老坛酸菜鱼",  CategoryId = 9, Price = 56.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/4a9cefba-6a74-467e-9fde-6e687ea725d7.png", Description = "原料：汤，草鱼，酸菜", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "经典酸菜鮰鱼", CategoryId = 9, Price = 66.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/5260ff39-986c-4a97-8850-2ec8c7583efc.png", Description = "原料：酸菜，江团，鮰鱼", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "蜀味水煮草鱼", CategoryId = 9, Price = 38.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/a6953d5a-4c18-4b30-9319-4926ee77261f.png", Description = "原料：草鱼，汤", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "清炒小油菜",   CategoryId = 8, Price = 18.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/3613d38e-5614-41c2-90ed-ff175bf50716.png", Description = "原料：小油菜", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "蒜蓉娃娃菜",   CategoryId = 8, Price = 18.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/4879ed66-3860-4b28-ba14-306ac025fdec.png", Description = "原料：蒜，娃娃菜", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "清炒西兰花",   CategoryId = 8, Price = 18.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/e9ec4ba4-4b22-4fc8-9be0-4946e6aeb937.png", Description = "原料：西兰花", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "炝炒圆白菜",   CategoryId = 8, Price = 18.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/22f59feb-0d44-430e-a6cd-6a49f27453ca.png", Description = "原料：圆白菜", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "清蒸鲈鱼",    CategoryId = 7, Price = 98.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/c18b5c67-3b71-466c-a75a-e63c6449f21c.png", Description = "原料：鲈鱼", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "东坡肘子",    CategoryId = 7, Price = 138.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/a80a4b8c-c93e-4f43-ac8a-856b0d5cc451.png", Description = "原料：猪肘棒", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "梅菜扣肉",    CategoryId = 7, Price = 58.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/6080b118-e30a-4577-aab4-45042e3f88be.png", Description = "原料：猪肉，梅菜", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "剁椒鱼头", CategoryId = 7, Price = 66.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/13da832f-ef2c-484d-8370-5934a1045a06.png", Description = "原料：鲢鱼，剁椒", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "金汤酸菜牛蛙", CategoryId = 6, Price = 88.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/7694a5d8-7938-4e9d-8b9e-2075983a2e38.png", Description = "原料：鲜活牛蛙，酸菜", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "香锅牛蛙", CategoryId = 6, Price = 88.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/f5ac8455-4793-450c-97ba-173795c34626.png", Description = "配料：鲜活牛蛙，莲藕，青笋", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "馋嘴牛蛙", CategoryId = 6, Price = 88.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/7a55b845-1f2b-41fa-9486-76d187ee9ee1.png", Description = "配料：鲜活牛蛙，丝瓜，黄豆芽", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "草鱼2斤", CategoryId = 5, Price = 68.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/b544d3ba-a1ae-4d20-a860-81cb5dec9e03.png", Description = "原料：草鱼，黄豆芽，莲藕", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "江团鱼2斤", CategoryId = 5, Price = 119.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/a101a1e9-8f8b-47b2-afa4-1abd47ea0a87.png", Description = "配料：江团鱼，黄豆芽，莲藕", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "鮰鱼2斤", CategoryId = 5, Price = 72.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/8cfcc576-4b66-4a09-ac68-ad5b273c2590.png", Description = "原料：鮰鱼，黄豆芽，莲藕", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "鸡蛋汤", CategoryId = 10, Price = 4.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/c09a0ee8-9d19-428d-81b9-746221824113.png", Description = "配料：鸡蛋，紫菜", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1},
                new Dish{ Name = "平菇豆腐汤", CategoryId = 10, Price = 6.00m,Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/16d0a3d6-2253-4cfc-9b49-bf7bd9eb2ad2.png", Description = "配料：豆腐，平菇", Status = 1, CreateTime = DateTime.Now, UpdateTime = DateTime.Now,CreateUser = 1,UpdateUser = 1}
            };
            var dishRepository = _unitOfWork.GetBaseRepository<Dish>();
            await dishRepository.AddRangeAsync(dishList);
            await _unitOfWork.SaveChangesAsync();
            return Ok("菜品数据添加成功");
        }


    }
}
