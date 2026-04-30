using MediatR;

namespace SD_Server.Application.Features.Professionals.Commands.GetAll
{
    public class GetProfessionalsCommand : IRequest<IEnumerable<ProfessionalDTO>>
    {
    }
}
