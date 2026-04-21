using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD_Api_Base.Base;
using SD_Server.Application.Features.Professionals.Commands.Create;

namespace SD_Server.Api.Controllers.Professional
{
    [ApiController]
    [Route("[controller]")]
    public class ProfessionalController(IMediator mediator, IMapper mapper) : ApiControllerBase(mapper)
    {
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProfessionalCommand request)
        {
            var result = await mediator.Send(request);

            if (result.IsFailure)
                return HandleCommand(result);

            var dto = result.Success;

            return Created(string.Empty, dto);
        }
    }
}
