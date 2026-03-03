using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PrestaColombia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("privado")]
        public IActionResult Privado()
        {
            return Ok("Si ves esto, estás autenticado 🔐");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("solo-admin")]
        public IActionResult SoloAdmin()
        {
            return Ok("Si ves esto, eres ADMIN 👑");
        }
    }
}