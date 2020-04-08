using Microsoft.AspNetCore.Mvc;

namespace Verivox.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get()
        {
            return Content("API Is Up");
        }
    }
}
