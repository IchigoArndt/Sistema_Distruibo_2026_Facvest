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
using System.Security.Claims;

namespace SD_Server.Api.Controllers.Avaliation
{
    [ApiController]
    [Route("[controller]")]
    public class AvaliationController(IMediator mediator, IMapper mapper) : ApiControllerBase(mapper)
    {
        [HttpPost("RequestAssessment/{professionalId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RequestAssessment(int professionalId, [FromBody] RequestAssessmentDTO dto)
        {
            var entityIdClaim = User.FindFirstValue("entity_id");
            if (!int.TryParse(entityIdClaim, out var studentId))
                return BadRequest("Usuário inválido.");

            var command = new AvaliationCreateCommand
            {
                StudentId        = studentId,
                ProfessionalId   = professionalId,
                Date             = dto.Date,
                TypeAvaliation   = dto.TypeAvaliation,
                StudentObjective = dto.StudentObjective,
                Status           = SD_Server.Domain.Enum.StatusAssessmentEnum.Pending,
            };

            var result = await mediator.Send(command);
            return HandleCommand(result);
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Professional")]
        public async Task<IActionResult> Create([FromBody] AvaliationCreateCommand request)
        {
            var result = await mediator.Send(request);
            return HandleCommand(result);
        }

        [HttpGet("GetByStudent")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetByStudent()
        {
            var entityIdClaim = User.FindFirstValue("entity_id");
            if (!int.TryParse(entityIdClaim, out var studentId))
                return BadRequest("Usuário inválido.");

            var query = new AvaliationByStudentHandler.Query { StudentId = studentId };
            var result = await mediator.Send(query);
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
        [Authorize(Roles = "Admin,Professional,Student")]
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
