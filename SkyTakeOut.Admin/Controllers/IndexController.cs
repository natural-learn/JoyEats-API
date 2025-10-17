using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SkyTakeOut.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {

        [HttpGet("hello")]
        public IActionResult Hello()
        {
            throw new ArgumentNullException("参数为空");
        }
    }
}
