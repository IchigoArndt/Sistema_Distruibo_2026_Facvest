using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD_Api_Base.Base;
using SD_Server.Application.Features.Assessments.Commands.Create;
using SD_Server.Application.Features.Assessments.Handlers;

namespace SD_Server.Api.Controllers.Assessment
{
    [ApiController]
    [Route("[controller]")]
    public class AssessmentController(IMediator mediator, IMapper mapper) : ApiControllerBase(mapper)
    {
        [HttpPost("Create")]
        [Authorize(Roles = "Admin,Professional")]
        public async Task<IActionResult> Create(AssessmentCreateCommand request)
        {
            var result = await mediator.Send(request);

            if (result.IsFailure)
                return HandleCommand(result);

            return Created(string.Empty, result.Success);
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var query = new AssessmentCollectionHandler.Query();
            var response = await mediator.Send(query);
            return HandleCommand(response);
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Admin,Professional,Student")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new AssessmentDetailHandler.Query() { Id = id };
            var response = await mediator.Send(query);
            return HandleCommand(response);
        }
    }
}
