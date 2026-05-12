using MediatR;
using SD_Server.Application.Features.Professionals.DTO;
using SD_SharedKernel.Helpers;

namespace SD_Server.Application.Features.Professionals.Queries
{
    public class GetAllProfessionalsQuery : IRequest<Result<Exception, List<ProfessionalDTO>>>
    {
    }
}