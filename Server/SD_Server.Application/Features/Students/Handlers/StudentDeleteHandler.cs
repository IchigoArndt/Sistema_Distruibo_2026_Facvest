using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Students.Commands.Delete;
using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Exceptions;
using SD_Server.Domain.Features.Students;
using SD_Server.Domain.Features.Users;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Students.Handlers
{
    public class StudentDeleteHandler
    {
        public class Handler(
            ILogger<Handler> logger,
            IStudentRepository repository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork) : IRequestHandler<StudentDeleteCommand, Result<Exception, Unit>>
        {
            public async Task<Result<Exception, Unit>> Handle(StudentDeleteCommand request, CancellationToken cancellationToken)
            {
                var validator = new StudentDeleteCommandValidator();
                var resultValidate = validator.Validate(request);

                if (!resultValidate.IsValid)
                {
                    var messageError = resultValidate.Errors.FirstOrDefault().ErrorMessage;
                    logger.LogError("Ocorreu um erro na validação do comando. Erro: {Error}", messageError);
                    return new Exception(messageError);
                }

                var studentResult = await repository.GetByIdAsync(request.Id);

                if (studentResult.IsFailure)
                {
                    logger.LogError("Ocorreu uma falha ao buscar o aluno com o id {Id}. Erro: {Erro}", request.Id, studentResult.Failure.Message);
                    return new Exception(studentResult.Failure.Message);
                }

                if (studentResult.Success == null)
                {
                    logger.LogError("Não foi encontrado nenhum aluno com o id {Id}", request.Id);
                    return new BusinessException(ErrorCodes.NotFound, $"Não foi encontrado nenhum aluno com o Id {request.Id}");
                }

                var userResult = await userRepository.GetByEntityId(request.Id, TypeUserEnum.Student);

                if (userResult.IsFailure)
                {
                    logger.LogError("Não foi encontrado o usuário vinculado ao aluno com id {Id}. Erro: {Erro}", request.Id, userResult.Failure.Message);
                    return new Exception(userResult.Failure.Message);
                }

                await unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    logger.LogInformation("Excluindo logicamente o aluno com id {Id}", request.Id);

                    var deleteStudentResult = await repository.DeleteAsync(studentResult.Success);

                    if (deleteStudentResult.IsFailure)
                    {
                        logger.LogError("Erro ao excluir logicamente o aluno com id {Id}. Erro: {Erro}", request.Id, deleteStudentResult.Failure.Message);
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return deleteStudentResult.Failure;
                    }

                    logger.LogInformation("Excluindo logicamente o usuário vinculado ao aluno com id {Id}", request.Id);

                    var deleteUserResult = await userRepository.DeleteAsync(userResult.Success);

                    if (deleteUserResult.IsFailure)
                    {
                        logger.LogError("Erro ao excluir logicamente o usuário vinculado ao aluno com id {Id}. Erro: {Erro}", request.Id, deleteUserResult.Failure.Message);
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return deleteUserResult.Failure;
                    }

                    await unitOfWork.CommitAsync(cancellationToken);

                    logger.LogInformation("Aluno e usuário com id {Id} excluídos com sucesso", request.Id);

                    return Unit.Sucessful;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro inesperado ao excluir aluno e usuário. Realizando rollback.");
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return ex;
                }
            }
        }
    }
}
