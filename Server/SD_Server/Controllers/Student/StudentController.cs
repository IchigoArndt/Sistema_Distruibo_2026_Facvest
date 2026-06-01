using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD_Api_Base.Base;
using SD_Server.Application.Features.Students.Commands.Create;
using SD_Server.Application.Features.Students.Commands.Delete;
using SD_Server.Application.Features.Students.Commands.Detail;
using SD_Server.Application.Features.Students.Commands.Edit;
using SD_Server.Application.Features.Students.Handlers;
using System.Security.Claims;

namespace SD_Server.Api.Controllers.Student
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController(IMediator mediator, IMapper mapper) : ApiControllerBase(mapper)
    {
        [HttpGet("GetMe")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMe()
        {
            var entityIdClaim = User.FindFirstValue("entity_id");
            if (!int.TryParse(entityIdClaim, out var studentId))
                return BadRequest("Usuário inválido.");

            var query = new StudentDetailCommand { Id = studentId };
            var response = await mediator.Send(query);
            return HandleCommand(response);
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin,Professional")]
        public async Task<IActionResult> Create(StudentCreateCommand request)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role == "Professional")
            {
                var entityIdClaim = User.FindFirstValue("entity_id");
                if (int.TryParse(entityIdClaim, out var professionalId))
                    request.ProfessionalId = professionalId;
            }
            
            var result = await mediator.Send(request);

            return HandleCommand(result);
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var query = new StudentCollectionHandler.Query();

            var response = await mediator.Send(query);

            return HandleCommand(response);
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Admin,Professional")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new StudentDetailCommand() { Id = id };

            var response = await mediator.Send(query);

            return HandleCommand(response);
        }

        [HttpPut("UpdateStudent/{id}")]
        [Authorize(Roles = "Professional")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentEditCommand command)
        {
            command.Id = id;

            var response = await mediator.Send(command);

            return HandleCommand(response);
        }

        [HttpDelete("DeleteStudent/{id}")]
        [Authorize(Roles = "Admin,Professional")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var query = new StudentDeleteCommand() { Id = id };

            var response = await mediator.Send(query);

            return HandleCommand(response);
        }
        
        [HttpGet("GetAllStudentsByProfessionalId")]
        [Authorize(Roles = "Professional")]
        public async Task<IActionResult> GetAllStudentsByProfessionalId()
        {
            var query = new StudentCollectionHandler.QueryStudent();
            
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role == "Professional")
            {
                var entityIdClaim = User.FindFirstValue("entity_id");
                if (int.TryParse(entityIdClaim, out var professionalId))
                    query.ProfessionalId  = professionalId;
            }
                
            var response = await mediator.Send(query);

            return HandleCommand(response);
        }
    }
}
