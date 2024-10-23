using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Api3Controller : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(2000); // Artificial delay
            return Ok("Response from API 2");
        }
    }
}
