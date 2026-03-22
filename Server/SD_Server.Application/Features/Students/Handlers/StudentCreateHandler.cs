using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Students.Commands.Create;
using SD_Server.Application.Helpers;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Students;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Students.Handlers
{
    public class StudentCreateHandler
    {
        public class Handler(ILogger<Handler> logger, IStudentRepository repository) : IRequestHandler<StudentCreateCommand, Result<Exception, Unit>>
        {
            public async Task<Result<Exception, Unit>> Handle(StudentCreateCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Iniciando o processo de criação do aluno com o nome: {Name}", request.Name);

                var validationResult = ValidationHelper<StudentCreateCommandValidator, StudentCreateCommand>.Validate(new StudentCreateCommandValidator(), request);
                if (validationResult.IsFailure)
                    return validationResult.Failure;

                var student = new Student();

                var mappingResult = MapFrom(request, ref student);

                if (mappingResult.IsFailure)
                {
                    logger.LogError($"Erro ao mapear o comando para a entidade Student. Detalhes: {mappingResult.Failure.Message}");
                    return new Exception(mappingResult.Failure.Message);
                }

                student = mappingResult.Success;

                var createResult = await repository.AddAsync(student);

                if(createResult.IsFailure)
                {
                    logger.LogError($"Erro ao criar o aluno no repositório. Detalhes: {createResult.Failure.Message}");

                    return new Exception(createResult.Failure.Message);
                }

                logger.LogInformation("Aluno criado com sucesso. Id: {id}", createResult.Success);

                return Unit.Sucessful;
            }

            private Result<Exception, Student> MapFrom(StudentCreateCommand src, ref Student dest)
            {
                if (src == null)
                    return new Exception();

                var destReturn = new Student
                {
                    Age = src.Age,
                    CellPhone = src.CellPhone,
                    Email = src.Email,
                    Name = src.Name,
                    Status = StatusEnum.Active,
                };

                return destReturn;
            }
        }
    }
}
