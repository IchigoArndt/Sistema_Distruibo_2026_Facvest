using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Avaliations.Commands.Create;
using SD_Server.Application.Helpers;
using SD_Server.Domain.Features.Avaliations;
using SD_Server.Domain.Features.Professionals;
using SD_Server.Domain.Features.Students;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Avaliations.Handlers
{
    public class AvaliationCreateHandler
    {
        public class Handler(ILogger<Handler> logger, IAvaliationRepository avaliationRepository, IStudentRepository studentRepository, IProfessionalRepository professionalRepository) : IRequestHandler<AvaliationCreateCommand, Result<Exception, Unit>>
        {
            public async Task<Result<Exception, Unit>> Handle(AvaliationCreateCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    logger.LogInformation($"Iniciando o processo de criação da avaliação do Usuario {request.StudentId} com o profisional {request.ProfessionalId}");

                    var validationResult = ValidationHelper<AvaliationCreateCommandValidator, AvaliationCreateCommand>.Validate(new AvaliationCreateCommandValidator(), request);
                    if (validationResult.IsFailure)
                        return validationResult.Failure;

                    logger.LogInformation($"Vericando a existencia do usuario na base de dados");
                    //Verificar se o estudante existe
                    var student = await studentRepository.GetByIdAsync(request.StudentId);

                    if (student.IsFailure || student.Success == null)
                        return new Exception($"Estudante com id {request.StudentId} não encontrado");

                    logger.LogInformation($"Usuario encontrado com o nome: {student.Success.Name}");

                    logger.LogInformation($"Vericando a existencia do profissional na base de dados");
                    //Verificar se o profissional existe
                    var professional = await professionalRepository.GetByIdAsync(request.ProfessionalId);

                    if (professional.IsFailure || professional.Success == null)
                        return new Exception($"Profissional com id {request.ProfessionalId} não encontrado");

                    logger.LogInformation($"Profissional encontrado com o nome: {professional.Success.Name}");

                    var avaliationResult = MapToAvaliation(request);

                    if (avaliationResult.IsFailure)
                        return avaliationResult.Failure;

                    var avaliation = avaliationResult.Success;

                    await avaliationRepository.AddAsync(avaliation);

                    logger.LogInformation($"Avaliação criada com sucesso para o usuario {request.StudentId} com o profissional {request.ProfessionalId}");

                    return Unit.Sucessful;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro inesperado ao criar a avaliação.");
                    return new Exception(ex.ToString());
                }
            }

            private Result<Exception, Avaliation> MapToAvaliation(AvaliationCreateCommand request)
            {
                try
                {
                    return new Avaliation
                    {
                        StudentId = request.StudentId,
                        ProfessionalId = request.ProfessionalId,
                        Date = request.Date,
                        TypeAvaliation = request.TypeAvaliation,
                        StudentObjective = request.StudentObjective,
                        Status = request.Status,
                        BodyComposition = request.BodyComposition,
                        Skinfolds = request.Skinfolds,
                        Anamnesis = request.Anamnesis,
                        TechnicalOpinion = request.TechnicalOpinion,
                        DateNextAvaliation = request.DateNextAvaliation,
                        IMC = request.IMC,
                        BodyFatPercentage = request.BodyFatPercentage
                    };
                }
                catch(Exception ex)
                {
                    return new Exception($"Erro ao mapear a avaliação: {ex.Message}");
                }
            }
        }
    }
}
