using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Professionals.Commands.Create;
using SD_Server.Application.Features.Professionals.DTO;
using SD_Server.Application.Helpers;
using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Exceptions;
using SD_Server.Domain.Features.Professionals;
using SD_Server.Domain.Features.Users;
using SD_SharedKernel.Helpers;
using BC = BCrypt.Net.BCrypt;

namespace SD_Server.Application.Features.Professionals.Handlers
{
    public class ProfessionalCreateHandler
    {
        public class Handler(
            ILogger<Handler> logger,
            IProfessionalRepository repository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork) : IRequestHandler<CreateProfessionalCommand, Result<Exception, ProfessionalDTO>>
        {
            public async Task<Result<Exception, ProfessionalDTO>> Handle(CreateProfessionalCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Iniciando criação de profissional: {Name}", request.Name);

                var validationResult = ValidationHelper<CreateProfessionalCommandValidator, CreateProfessionalCommand>.Validate(new CreateProfessionalCommandValidator(), request);
                if (validationResult.IsFailure)
                    return validationResult.Failure;

                await unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    var professionalsResult = await repository.GetAllAsync();
                    if (professionalsResult.IsFailure)
                    {
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return professionalsResult.Failure;
                    }

                    var existsEmail = professionalsResult.Success.Any(x => x.Email == request.Email);
                    if (existsEmail)
                    {
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return new BusinessException(ErrorCodes.AlreadyExists, "Email já cadastrado.");
                    }

                    var existsCref = professionalsResult.Success.Any(x => x.Cref == request.Cref);
                    if (existsCref)
                    {
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return new BusinessException(ErrorCodes.AlreadyExists, "CREF já cadastrado.");
                    }

                    var professional = new Professional
                    {
                        Name = request.Name,
                        Email = request.Email,
                        Phone = request.Phone,
                        Cref = request.Cref,
                        Bio = request.Bio,
                        PasswordHash = BC.HashPassword(request.Password),
                        Status = StatusEnum.Active
                    };

                    var createProfessionalResult = await repository.AddAsync(professional);

                    if (createProfessionalResult.IsFailure)
                    {
                        logger.LogError("Erro ao criar profissional no repositório. Detalhes: {Message}", createProfessionalResult.Failure.Message);
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return createProfessionalResult.Failure;
                    }

                    var user = new User
                    {
                        Email = request.Email,
                        Name = request.Name,
                        Status = StatusEnum.Active,
                        TypeAccess = TypeUserEnum.Professional,
                        CreatedAt = DateTime.UtcNow,
                        EntityId = createProfessionalResult.Success,
                        PasswordHash = professional.PasswordHash
                    };

                    var createUserResult = await userRepository.AddAsync(user);

                    if (createUserResult.IsFailure)
                    {
                        logger.LogError("Erro ao criar usuário no repositório. Detalhes: {Message}", createUserResult.Failure.Message);
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return createUserResult.Failure;
                    }

                    await unitOfWork.CommitAsync(cancellationToken);

                    var dto = new ProfessionalDTO
                    {
                        Id = createProfessionalResult.Success,
                        Name = professional.Name,
                        Email = professional.Email,
                        Phone = professional.Phone,
                        Cref = professional.Cref,
                        Bio = professional.Bio,
                        Status = professional.Status
                    };

                    return dto;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro inesperado ao criar profissional e usuário. Realizando rollback.");
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return ex;
                }
            }
        }
    }
}
