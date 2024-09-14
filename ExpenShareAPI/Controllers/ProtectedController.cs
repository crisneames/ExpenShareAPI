using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetProtectedData()
        {
            return Ok(new { message = "This is protected data" });
        }
    }
}
