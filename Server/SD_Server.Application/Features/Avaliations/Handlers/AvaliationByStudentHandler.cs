using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Avaliations.DTO;
using SD_Server.Domain.Features.Avaliations;
using SD_SharedKernel.Helpers;

namespace SD_Server.Application.Features.Avaliations.Handlers
{
    public class AvaliationByStudentHandler
    {
        public class Query : IRequest<Result<Exception, IQueryable<AvaliationDTO>>>
        {
            public int StudentId { get; set; }
        }

        public class HandlerQuery(
            ILogger<HandlerQuery> logger,
            IAvaliationRepository repository) : IRequestHandler<Query, Result<Exception, IQueryable<AvaliationDTO>>>
        {
            public async Task<Result<Exception, IQueryable<AvaliationDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Buscando avaliações do aluno Id: {StudentId}", request.StudentId);

                var result = await repository.GetByStudentIdAsync(request.StudentId);

                if (result.IsFailure)
                {
                    logger.LogError("Falha ao buscar avaliações do aluno Id: {StudentId}. Erro: {Erro}", request.StudentId, result.Failure.Message);
                    return new Exception(result.Failure.Message);
                }

                if (!result.Success.Any())
                {
                    logger.LogInformation("Nenhuma avaliação encontrada para o aluno Id: {StudentId}", request.StudentId);
                    return Result<Exception, IQueryable<AvaliationDTO>>.Of(new List<AvaliationDTO>().AsQueryable());
                }

                logger.LogInformation("Avaliações encontradas para o aluno Id: {StudentId}. Quantidade: {Qtd}", request.StudentId, result.Success.Count());

                var dtos = result.Success.Select(a => new AvaliationDTO
                {
                    Id                 = a.Id,
                    StudentId          = a.StudentId,
                    ProfessionalId     = a.ProfessionalId,
                    ProfessionalName   = a.Professional != null ? a.Professional.Name : null,
                    Date               = a.Date,
                    TypeAvaliation     = a.TypeAvaliation,
                    StudentObjective   = a.StudentObjective,
                    Status             = a.Status,
                    BodyComposition    = a.BodyComposition,
                    Skinfolds          = a.Skinfolds,
                    Anamnesis          = a.Anamnesis,
                    TechnicalOpinion   = a.TechnicalOpinion,
                    DateNextAvaliation = a.DateNextAvaliation,
                    IMC                = a.IMC,
                    BodyFatPercentage  = a.BodyFatPercentage
                });

                return Result<Exception, IQueryable<AvaliationDTO>>.Of(dtos);
            }
        }
    }
}
