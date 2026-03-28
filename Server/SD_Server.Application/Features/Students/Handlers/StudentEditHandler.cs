using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Students.Commands.Edit;
using SD_Server.Domain.Exceptions;
using SD_Server.Domain.Features.Students;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Students.Handlers
{
    public class StudentEditHandler
    {
        public class Handle(ILogger<Handle> logger, IStudentRepository repository) : IRequestHandler<StudentEditCommand, Result<Exception, Unit>>
        {
            async Task<Result<Exception, Unit>> IRequestHandler<StudentEditCommand, Result<Exception, Unit>>.Handle(StudentEditCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Iniciando processo de atualização do aluno com o Id {id}", request.Id);

                var validator = new StudentEditCommandValidator();

                var validateResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validateResult.IsValid)
                {
                    var errorMessage = validateResult.Errors.FirstOrDefault().ErrorMessage;

                    logger.LogError("Falha durante a validação do commando para atualização do aluno com id {}. Erro: {erro}", request.Id, errorMessage);

                    return new Exception(errorMessage);
                }

                var studentResult = await repository.GetByIdAsync(request.Id);

                if (studentResult.IsFailure)
                {
                    logger.LogError("Falha durante a busca do aluno com o Id {id}. Erro: {erro}", request.Id, studentResult.Failure.Message);

                    return new Exception(studentResult.Failure.Message);
                }

                if (studentResult.Success == null)
                {
                    logger.LogWarning("Falha, não encontrada o aluno com o Id {id}", request.Id);

                    return new BusinessException(ErrorCodes.NotFound, "Falha aluno não encontrado");
                }

                var student = studentResult.Success;

                var mapResult = await MapFrom(request, student);

                if (mapResult.IsFailure)
                {
                    logger.LogError("Falha ao criar o objeto para atualização. Erro: {erro}", mapResult.Failure.Message);

                    return new Exception(mapResult.Failure.Message);
                }

                var updateResult = await repository.UpdateAsync(mapResult.Success);

                if (updateResult.IsFailure)
                {
                    logger.LogError("Falha durante a atualização do aluno com o Id {id}. Erro: {erro}", request.Id, studentResult.Failure.Message);

                    return new Exception(studentResult.Failure.Message);
                }

                return Unit.Sucessful;
            }

            private async Task<Result<Exception, Student>> MapFrom(StudentEditCommand request, Student student)
            {
                try
                {
                    student.Name = request.Name;
                    student.Email = request.Email;

                    if(request.Age != null)
                        student.Age = request.Age.Value;
                    else
                        student.Age = 0;

                    student.CellPhone = request.CellPhone;
                    return student;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
        }
    }
}