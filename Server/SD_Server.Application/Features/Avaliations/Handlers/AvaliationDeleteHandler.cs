using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Avaliations.Commands.Delete;
using SD_Server.Application.Helpers;
using SD_Server.Domain.Base;
using SD_Server.Domain.Exceptions;
using SD_Server.Domain.Features.Avaliations;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Avaliations.Handlers
{
    public class AvaliationDeleteHandler
    {
        public class Handler(
            ILogger<Handler> logger,
            IAvaliationRepository repository,
            IUnitOfWork unitOfWork) : IRequestHandler<AvaliationDeleteCommand, Result<Exception, Unit>>
        {
            public async Task<Result<Exception, Unit>> Handle(AvaliationDeleteCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Iniciando exclusão lógica da avaliação Id: {Id}", request.Id);

                var validationResult = ValidationHelper<AvaliationDeleteCommandValidator, AvaliationDeleteCommand>.Validate(new AvaliationDeleteCommandValidator(), request);
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
                    var deleteResult = await repository.DeleteAsync(avaliationResult.Success);

                    if (deleteResult.IsFailure)
                    {
                        logger.LogError("Erro ao excluir avaliação Id: {Id}. Erro: {Erro}", request.Id, deleteResult.Failure.Message);
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return deleteResult.Failure;
                    }

                    await unitOfWork.CommitAsync(cancellationToken);

                    logger.LogInformation("Avaliação Id: {Id} excluída com sucesso.", request.Id);
                    return Unit.Sucessful;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro inesperado ao excluir avaliação Id: {Id}. Realizando rollback.", request.Id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return ex;
                }
            }
        }
    }
}
