using AutoMapper;
using MediatR;
using SD_Server.Application.Features.Professionals.Commands;
using SD_Server.Application.Features.Professionals.DTO;
using SD_Server.Domain.Features.Professionals;
using SD_SharedKernel.Helpers;

namespace SD_Server.Application.Features.Professionals.Handlers
{
    public class ProfessionalCollectionHandler(
        IProfessionalRepository professionalRepository,
        IMapper mapper) : IRequestHandler<GetAllProfessionalsCommand, Result<Exception, List<ProfessionalDTO>>>
    {
        public async Task<Result<Exception, List<ProfessionalDTO>>> Handle(GetAllProfessionalsCommand request, CancellationToken cancellationToken)
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