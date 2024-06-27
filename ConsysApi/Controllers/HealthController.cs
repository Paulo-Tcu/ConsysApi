using Microsoft.AspNetCore.Mvc;

namespace ConsysApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Health()
        {
            return Ok(new { Message = "API Online =)" });
        }
    }
}
