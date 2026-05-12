using AutoMapper;
using MediatR;
using SD_Server.Application.Features.Professionals.Commands;
using SD_Server.Application.Features.Professionals.DTO;
using SD_Server.Domain.Features.Professionals;
using SD_SharedKernel.Helpers;

namespace SD_Server.Application.Features.Professionals.Handlers
{
    public class ProfessionalDetailHandler(
        IProfessionalRepository professionalRepository,
        IMapper mapper) : IRequestHandler<GetProfessionalByIdCommand, Result<Exception, ProfessionalDTO>>
    {
        public async Task<Result<Exception, ProfessionalDTO>> Handle(GetProfessionalByIdCommand request, CancellationToken cancellationToken)
        {
            var result = await professionalRepository.GetByIdAsync(request.Id);

            if (result.IsFailure)
                return result.Failure;

            var professional = result.Success;
            var dto = mapper.Map<ProfessionalDTO>(professional);
            return dto;
        }
    }
}