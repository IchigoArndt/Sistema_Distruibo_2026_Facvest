using AutoMapper;
using MediatR;
using SD_Server.Application.Features.Professionals.DTO;
using SD_Server.Application.Features.Professionals.Queries;
using SD_Server.Domain.Features.Professionals;
using SD_SharedKernel.Helpers;

namespace SD_Server.Application.Features.Professionals.Handlers
{
    public class GetAllProfessionalsQueryHandler(
        IProfessionalRepository professionalRepository,
        IMapper mapper) : IRequestHandler<GetAllProfessionalsQuery, Result<Exception, List<ProfessionalDTO>>>
    {
        public async Task<Result<Exception, List<ProfessionalDTO>>> Handle(GetAllProfessionalsQuery request, CancellationToken cancellationToken)
        {
            var result = await professionalRepository.GetAllAsync();

            if (result.IsFailure)
                return result.Failure;

            var professionals = result.Success.ToList();
            var dtos = mapper.Map<List<ProfessionalDTO>>(professionals);
            return dtos;
        }
    }
}