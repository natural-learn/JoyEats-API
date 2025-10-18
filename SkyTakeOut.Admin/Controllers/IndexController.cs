using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SkyTakeOut.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly ILogger<IndexController> _logger;

        public IndexController(ILogger<IndexController> logger)
        {
            _logger = logger;
        }

        [HttpGet("hello")]
        public IActionResult Hello()
        {
            _logger.LogInformation($"测试日志记录功能成功!!!");
            return Ok("请求成功");
        }
    }
}
