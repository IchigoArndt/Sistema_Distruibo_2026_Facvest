using MediatR;
using SD_Server.Application.Features.Professionals.DTO;
using SD_SharedKernel.Helpers;

namespace SD_Server.Application.Features.Professionals.Queries
{
    public class GetProfessionalByIdQuery : IRequest<Result<Exception, ProfessionalDTO>>
    {
        public int Id { get; set; }
    }
}