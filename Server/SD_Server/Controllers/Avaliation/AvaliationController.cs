using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using SD_Api_Base.Base;
using SD_Server.Application.Features.Avaliations.Commands.Create;
using SD_Server.Application.Features.Avaliations.Commands.Delete;
using SD_Server.Application.Features.Avaliations.Commands.Detail;
using SD_Server.Application.Features.Avaliations.Commands.Edit;
using SD_Server.Application.Features.Avaliations.Handlers;

namespace SD_Server.Api.Controllers.Avaliation
{
    [ApiController]
    [Route("[controller]")]
    public class AvaliationController(IMediator mediator, IMapper mapper) : ApiControllerBase(mapper)
    {
        [HttpPost("Create")]
        //[Authorize(Roles = "Professional")]
        public async Task<IActionResult> Create([FromBody] AvaliationCreateCommand request)
        {
            var result = await mediator.Send(request);
            return HandleCommand(result);
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin,Professional")]
        public async Task<IActionResult> GetAll()
        {
            var query = new AvaliationCollectionHandler.Query();
            var result = await mediator.Send(query);
            return HandleCommand(result);
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Admin,Professional")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new AvaliationDetailCommand { Id = id };
            var result = await mediator.Send(query);
            return HandleCommand(result);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Professional")]
        public async Task<IActionResult> Update(int id, [FromBody] AvaliationEditCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command);
            return HandleCommand(result);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin,Professional")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new AvaliationDeleteCommand { Id = id };
            var result = await mediator.Send(command);
            return HandleCommand(result);
        }
    }
}
