using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD_Api_Base.Base;
using SD_Server.Application.Features.Professionals.Commands;
using SD_Server.Application.Features.Professionals.Commands.Create;
using SD_Server.Application.Features.Professionals.Commands.Delete;
using SD_Server.Application.Features.Professionals.Commands.Update;
using System.Security.Claims;

namespace SD_Server.Api.Controllers.Professional
{
    [ApiController]
    [Route("[controller]")]
    public class ProfessionalController(IMediator mediator, IMapper mapper) : ApiControllerBase(mapper)
    {
        [HttpGet("GetMe")]
        [Authorize(Roles = "Professional")]
        public async Task<IActionResult> GetMe()
        {
            var entityIdClaim = User.FindFirstValue("entity_id");
            if (!int.TryParse(entityIdClaim, out var professionalId))
                return BadRequest("Usuário inválido.");

            var command = new GetProfessionalByIdCommand { Id = professionalId };
            var result = await mediator.Send(command);
            return HandleCommand(result);
        }

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

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin,Professional,Student")]
        public async Task<IActionResult> GetAll()
        {
            var command = new GetAllProfessionalsCommand();
            var result = await mediator.Send(command);
            return HandleCommand(result);
        }

        [HttpGet("GetById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var command = new GetProfessionalByIdCommand { Id = id };
            var result = await mediator.Send(command);
            return HandleCommand(result);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin,Professional")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProfessionalCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command);
            return HandleCommand(result);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteProfessionalCommand { Id = id };
            var result = await mediator.Send(command);
            return HandleCommand(result);
        }
    }
}
