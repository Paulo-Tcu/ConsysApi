using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsysApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("/")]
        [AllowAnonymous]
        public IActionResult Health()
        {
            return Ok(new { Message = "API Online =)" });
        }
    }
}
