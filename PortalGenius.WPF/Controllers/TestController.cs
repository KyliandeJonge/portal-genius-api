
using Microsoft.AspNetCore.Mvc;

namespace PortalGenius.WPF.Controllers
{
    public class TestController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Demo()
        {
            return Ok("Dit is gevonden");
        }
    }
}
