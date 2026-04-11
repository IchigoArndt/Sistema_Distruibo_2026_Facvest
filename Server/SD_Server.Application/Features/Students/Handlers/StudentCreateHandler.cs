using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Students.Commands.Create;
using SD_Server.Application.Helpers;
using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Students;
using SD_Server.Domain.Features.Users;
using SD_SharedKernel.Helpers;
using BC = BCrypt.Net.BCrypt;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Students.Handlers
{
    public class StudentCreateHandler
    {
        public class Handler(
            ILogger<Handler> logger,
            IStudentRepository repository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork) : IRequestHandler<StudentCreateCommand, Result<Exception, Unit>>
        {
            public async Task<Result<Exception, Unit>> Handle(StudentCreateCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Iniciando o processo de criação do aluno com o nome: {Name}", request.Name);

                var validationResult = ValidationHelper<StudentCreateCommandValidator, StudentCreateCommand>.Validate(new StudentCreateCommandValidator(), request);
                if (validationResult.IsFailure)
                    return validationResult.Failure;

                await unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    var student = new Student
                    {
                        Age = request.Age,
                        CellPhone = request.CellPhone,
                        Email = request.Email,
                        Name = request.Name,
                        Status = StatusEnum.Active,
                    };

                    var createStudentResult = await repository.AddAsync(student);

                    if (createStudentResult.IsFailure)
                    {
                        logger.LogError("Erro ao criar o aluno no repositório. Detalhes: {Message}", createStudentResult.Failure.Message);
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return createStudentResult.Failure;
                    }

                    logger.LogInformation("Aluno criado com sucesso. Id: {Id}", createStudentResult.Success);

                    var user = new User
                    {
                        Email        = request.Email,
                        Name         = request.Name,
                        Status       = StatusEnum.Active,
                        TypeAccess   = TypeUserEnum.Student,
                        CreatedAt    = DateTime.UtcNow,
                        EntityId     = createStudentResult.Success,
                        PasswordHash = BC.HashPassword(request.Password)
                    };

                    var createUserResult = await userRepository.AddAsync(user);

                    if (createUserResult.IsFailure)
                    {
                        logger.LogError("Erro ao criar o usuário no repositório. Detalhes: {Message}", createUserResult.Failure.Message);
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return createUserResult.Failure;
                    }

                    await unitOfWork.CommitAsync(cancellationToken);

                    logger.LogInformation("Usuário criado com sucesso. Id: {Id}", createUserResult.Success);

                    return Unit.Sucessful;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro inesperado ao criar aluno e usuário. Realizando rollback.");
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return ex;
                }
            }
        }
    }
}
