using MediatR;
using SD_SharedKernel.Helpers;

namespace SD_Server.Application.Features.Professionals.Commands.Create
{
    public class CreateProfessionalCommand : IRequest<Result<Exception, SD_Server.Application.Features.Professionals.DTO.ProfessionalDTO>>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Cref { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
