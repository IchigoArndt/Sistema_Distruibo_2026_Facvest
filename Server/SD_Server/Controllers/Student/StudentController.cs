using Microsoft.AspNetCore.Mvc;
using SD_Api_Base.Base;
using AutoMapper;
using MediatR;
using SD_Server.Application.Features.Students.Commands.Create;

namespace SD_Server.Api.Controllers.Student
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController(IMediator mediator, IMapper mapper) : ApiControllerBase(mapper)
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create(StudentCreateCommand request)
        {
            var result = await mediator.Send(request);

            return HandleCommand(result);
        }
    }
}
