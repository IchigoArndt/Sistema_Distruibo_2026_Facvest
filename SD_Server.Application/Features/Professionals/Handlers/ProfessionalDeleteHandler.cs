using MediatR;
using SD_Server.Application.Features.Professionals.Commands.Delete;
using SD_Server.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("Professional")]
public class ProfessionalDeleteHandler : ControllerBase
{
    private readonly IProfessionalRepository _repository;

    public ProfessionalDeleteHandler(IProfessionalRepository repository)
    {
        _repository = repository;
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Handle(int id)
    {
        var professional = await _repository.GetByIdAsync(id);
        if (professional == null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
