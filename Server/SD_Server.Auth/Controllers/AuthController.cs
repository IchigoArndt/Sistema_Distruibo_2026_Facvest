using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SD_Api_Base.Base;
using SD_Server.Auth.Commands;

namespace SD_Server.Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IMediator mediator, IMapper mapper) : ApiControllerBase(mapper)
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var response = await mediator.Send(command);

            return HandleCommand(response);
        }
    }
}
