using MediatR;
using SD_Server.Application.Features.Professionals.Commands.GetAll;
using SD_Server.Application.Features.Professionals;
using SD_Server.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("Professional")]
public class ProfessionalCollectionHandler : ControllerBase
{
    private readonly IProfessionalRepository _repository;

    public ProfessionalCollectionHandler(IProfessionalRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<ProfessionalDTO>>> Handle()
    {
        var professionals = await _repository.GetAllAsync();
        var dtos = professionals.Select(p => new ProfessionalDTO
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            // PasswordHash is intentionally omitted
        }).ToList();
        return Ok(dtos);
    }
}
