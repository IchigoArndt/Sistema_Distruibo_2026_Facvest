using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Students.Commands.Edit;
using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Exceptions;
using SD_Server.Domain.Features.Students;
using SD_Server.Domain.Features.Users;
using SD_SharedKernel.Helpers;
using Crypt = BCrypt.Net.BCrypt;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Students.Handlers
{
    public class StudentEditHandler
    {
        public class Handle(
            ILogger<Handle> logger,
            IStudentRepository repository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork) : IRequestHandler<StudentEditCommand, Result<Exception, Unit>>
        {
            async Task<Result<Exception, Unit>> IRequestHandler<StudentEditCommand, Result<Exception, Unit>>.Handle(StudentEditCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Iniciando processo de atualização do aluno com o Id {Id}", request.Id);

                var validator = new StudentEditCommandValidator();
                var validateResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validateResult.IsValid)
                {
                    var errorMessage = validateResult.Errors.FirstOrDefault().ErrorMessage;
                    logger.LogError("Falha na validação do comando para atualização do aluno com id {Id}. Erro: {Erro}", request.Id, errorMessage);
                    return new Exception(errorMessage);
                }

                var studentResult = await repository.GetByIdAsync(request.Id);

                if (studentResult.IsFailure)
                {
                    logger.LogError("Falha ao buscar o aluno com o Id {Id}. Erro: {Erro}", request.Id, studentResult.Failure.Message);
                    return new Exception(studentResult.Failure.Message);
                }

                if (studentResult.Success == null)
                {
                    logger.LogWarning("Aluno com o Id {Id} não encontrado", request.Id);
                    return new BusinessException(ErrorCodes.NotFound, "Aluno não encontrado");
                }

                var atualizaSenha = !string.IsNullOrEmpty(request.Password);

                User? user = null;

                if (atualizaSenha)
                {
                    var userResult = await userRepository.GetByEntityId(request.Id, TypeUserEnum.Student);

                    if (userResult.IsFailure)
                    {
                        logger.LogError("Usuário vinculado ao aluno com id {Id} não encontrado. Erro: {Erro}", request.Id, userResult.Failure.Message);
                        return new Exception(userResult.Failure.Message);
                    }

                    user = userResult.Success;
                }

                await unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    var student = studentResult.Success;
                    student.Name      = request.Name ?? student.Name;
                    student.Email     = request.Email ?? student.Email;
                    student.Age       = request.Age ?? student.Age;
                    student.CellPhone = request.CellPhone ?? student.CellPhone;

                    var updateStudentResult = await repository.UpdateAsync(student);

                    if (updateStudentResult.IsFailure)
                    {
                        logger.LogError("Falha ao atualizar o aluno com o Id {Id}. Erro: {Erro}", request.Id, updateStudentResult.Failure.Message);
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return updateStudentResult.Failure;
                    }

                    if (atualizaSenha)
                    {
                        user!.PasswordHash = Crypt.HashPassword(request.Password);

                        var updateUserResult = await userRepository.UpdateAsync(user);

                        if (updateUserResult.IsFailure)
                        {
                            logger.LogError("Falha ao atualizar a senha do usuário vinculado ao aluno com id {Id}. Erro: {Erro}", request.Id, updateUserResult.Failure.Message);
                            await unitOfWork.RollbackAsync(cancellationToken);
                            return updateUserResult.Failure;
                        }

                        logger.LogInformation("Senha do usuário vinculado ao aluno com id {Id} atualizada com sucesso", request.Id);
                    }

                    await unitOfWork.CommitAsync(cancellationToken);

                    logger.LogInformation("Aluno com id {Id} atualizado com sucesso", request.Id);

                    return Unit.Sucessful;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro inesperado ao atualizar aluno. Realizando rollback.");
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return ex;
                }
            }
        }
    }
}