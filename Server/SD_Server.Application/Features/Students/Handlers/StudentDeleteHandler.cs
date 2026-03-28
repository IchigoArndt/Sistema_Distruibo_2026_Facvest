using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Students.Commands.Delete;
using SD_Server.Domain.Exceptions;
using SD_Server.Domain.Features.Students;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Students.Handlers
{
    public class StudentDeleteHandler
    {
        public class Handler(ILogger<Handler> logger, IStudentRepository repository) : IRequestHandler<StudentDeleteCommand, Result<Exception, Unit>>
        {
            public async Task<Result<Exception, Unit>> Handle(StudentDeleteCommand request, CancellationToken cancellationToken)
            {
                var validator = new StudentDeleteCommandValidator();

                var resultValidate = validator.Validate(request);

                if (!resultValidate.IsValid)
                {
                    var messageError = resultValidate.Errors.FirstOrDefault().ErrorMessage;

                    logger.LogError("Ocorreu um erro na validação do comando. Erro: {error}", messageError);

                    return new Exception(messageError);
                }

                var student = await repository.GetByIdAsync(request.Id);

                if (student.IsFailure)
                {
                    logger.LogError("Ocorreu uma falha ao buscar o aluno com o id {id}. Erro: {erro}", request.Id, student.Failure.Message);

                    return new Exception(student.Failure.Message);
                }

                if (student.Success == null)
                {
                    logger.LogError("Não foi encontrado nenhum aluno com o id {id}", request.Id);

                    return new BusinessException(ErrorCodes.NotFound, $"Não foi nenhum aluno com o Id {request.Id}");
                }

                logger.LogInformation("Excluindo logicamente o aluno com id {id}", request.Id);
                
                var resultDelete = await repository.DeleteAsync(student.Success);

                if(resultDelete.IsFailure)
                {
                    logger.LogError("Ocorreu um erro ao excluir logicamente o aluno com id {id}", request.Id);

                    return new Exception($"Ocorreu um erro ao excluir logicamente o aluno com id {request.Id}. Erro: {resultDelete.Failure.Message}");
                }

                return Unit.Sucessful;
            }
        }
    }
}
