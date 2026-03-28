using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SD_Api_Base.Base;
using SD_Server.Application.Features.Students.Commands.Create;
using SD_Server.Application.Features.Students.Commands.Delete;
using SD_Server.Application.Features.Students.Commands.Detail;
using SD_Server.Application.Features.Students.Commands.Edit;
using SD_Server.Application.Features.Students.Handlers;

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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var query = new StudentCollectionHandler.Query();

            var response = await mediator.Send(query);

            return HandleCommand(response);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new StudentDetailCommand() { Id = id };

            var response = await mediator.Send(query);

            return HandleCommand(response);
        }

        [HttpPut("UpdateStudent/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentEditCommand command)
        {
            command.Id = id;

            var response = await mediator.Send(command);

            return HandleCommand(response);
        }

        [HttpDelete("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var query = new StudentDeleteCommand() { Id = id };

            var response = await mediator.Send(query);

            return HandleCommand(response);
        }
    }
}
