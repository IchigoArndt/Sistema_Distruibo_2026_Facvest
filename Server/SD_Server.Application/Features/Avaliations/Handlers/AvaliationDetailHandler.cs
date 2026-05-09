using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Avaliations.Commands.Detail;
using SD_Server.Application.Features.Avaliations.DTO;
using SD_Server.Application.Helpers;
using SD_Server.Domain.Exceptions;
using SD_Server.Domain.Features.Avaliations;
using SD_SharedKernel.Helpers;

namespace SD_Server.Application.Features.Avaliations.Handlers
{
    public class AvaliationDetailHandler
    {
        public class Handler(
            ILogger<Handler> logger,
            IAvaliationRepository repository) : IRequestHandler<AvaliationDetailCommand, Result<Exception, AvaliationDTO>>
        {
            public async Task<Result<Exception, AvaliationDTO>> Handle(AvaliationDetailCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Buscando avaliação Id: {Id}", request.Id);

                var validationResult = ValidationHelper<AvaliationDetailCommandValidator, AvaliationDetailCommand>.Validate(new AvaliationDetailCommandValidator(), request);
                if (validationResult.IsFailure)
                    return validationResult.Failure;

                var avaliationResult = await repository.GetByIdAsync(request.Id);

                if (avaliationResult.IsFailure)
                {
                    logger.LogError("Falha ao buscar a avaliação Id: {Id}. Erro: {Erro}", request.Id, avaliationResult.Failure.Message);
                    return new Exception(avaliationResult.Failure.Message);
                }

                if (avaliationResult.Success == null)
                {
                    logger.LogWarning("Avaliação Id: {Id} não encontrada.", request.Id);
                    return new BusinessException(ErrorCodes.NotFound, $"Avaliação {request.Id} não encontrada.");
                }

                logger.LogInformation("Avaliação Id: {Id} encontrada.", request.Id);

                var a = avaliationResult.Success;
                var dto = new AvaliationDTO
                {
                    Id                  = a.Id,
                    StudentId           = a.StudentId,
                    ProfessionalId      = a.ProfessionalId,
                    Date                = a.Date,
                    TypeAvaliation      = a.TypeAvaliation,
                    StudentObjective    = a.StudentObjective,
                    Status              = a.Status,
                    BodyComposition     = a.BodyComposition,
                    Skinfolds           = a.Skinfolds,
                    Anamnesis           = a.Anamnesis,
                    TechnicalOpinion    = a.TechnicalOpinion,
                    DateNextAvaliation  = a.DateNextAvaliation,
                    IMC                 = a.IMC,
                    BodyFatPercentage   = a.BodyFatPercentage
                };

                return dto;
            }
        }
    }
}
