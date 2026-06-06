using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Avaliations.Commands.Edit;
using SD_Server.Application.Helpers;
using SD_Server.Domain.Base;
using SD_Server.Domain.Exceptions;
using SD_Server.Domain.Features.Avaliations;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Avaliations.Handlers
{
    public class AvaliationEditHandler
    {
        public class Handler(
            ILogger<Handler> logger,
            IAvaliationRepository repository,
            IUnitOfWork unitOfWork) : IRequestHandler<AvaliationEditCommand, Result<Exception, Unit>>
        {
            public async Task<Result<Exception, Unit>> Handle(AvaliationEditCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Iniciando processo de atualização da avaliação Id: {Id}", request.Id);

                var validationResult = ValidationHelper<AvaliationEditCommandValidator, AvaliationEditCommand>.Validate(new AvaliationEditCommandValidator(), request);
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

                await unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    var avaliation = avaliationResult.Success;

                    if (request.BodyComposition is not null)
                        avaliation.BodyComposition = request.BodyComposition;

                    if (request.Skinfolds is not null)
                        avaliation.Skinfolds = request.Skinfolds;

                    if (request.Anamnesis is not null)
                        avaliation.Anamnesis = request.Anamnesis;

                    if (!string.IsNullOrEmpty(request.TechnicalOpinion))
                        avaliation.TechnicalOpinion = request.TechnicalOpinion;

                    if (!string.IsNullOrEmpty(request.IMC))
                        avaliation.IMC = request.IMC;

                    if (!string.IsNullOrEmpty(request.BodyFatPercentage))
                        avaliation.BodyFatPercentage = request.BodyFatPercentage;

                    if (request.DateNextAvaliation.HasValue)
                        avaliation.DateNextAvaliation = request.DateNextAvaliation;

                    if (request.Status.HasValue)
                        avaliation.Status = request.Status.Value;

                    var updateResult = await repository.UpdateAsync(avaliation);

                    if (updateResult.IsFailure)
                    {
                        logger.LogError("Falha ao atualizar a avaliação Id: {Id}. Erro: {Erro}", request.Id, updateResult.Failure.Message);
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return updateResult.Failure;
                    }

                    await unitOfWork.CommitAsync(cancellationToken);

                    logger.LogInformation("Avaliação Id: {Id} atualizada com sucesso.", request.Id);
                    return Unit.Sucessful;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro inesperado ao atualizar avaliação Id: {Id}. Realizando rollback.", request.Id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return ex;
                }
            }
        }
    }
}
