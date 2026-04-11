using Microsoft.AspNetCore.Mvc;

namespace SD_Server.Api.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Email == "teste@ndd.com" && request.Password == "123456")
            {
                return Ok(new { Token = "token-gerado-bearer-token" });
            }

            return Unauthorized();
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}