using MediatR;
using Microsoft.Extensions.Logging;
using SD_SharedKernel.Helpers;
using SD_Server.Application.Features.Assessments.DTO;
using SD_Server.Domain.Features.Assessments;

namespace SD_Server.Application.Features.Assessments.Handlers
{
    public class AssessmentDetailHandler
    {
        public class Query : IRequest<Result<Exception, AssessmentDTO>>
        {
            public int Id { get; set; }
        }

        public class Handler(ILogger<Handler> logger, IAssessmentRepository repository) : IRequestHandler<Query, Result<Exception, AssessmentDTO>>
        {
            public async Task<Result<Exception, AssessmentDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await repository.GetByIdAsync(request.Id);
                if (result.IsFailure) return result.Failure;

                var a = result.Success;

                var dto = new AssessmentDTO
                {
                    Id = a.Id,
                    StudentId = a.StudentId,
                    ProfessionalId = a.ProfessionalId,
                    Date = a.Date,
                    Status = a.Status,
                    Methodology = a.Methodology,
                    Price = a.Price,
                    Notes = a.Notes,
                    Results = a.Results
                };

                return dto;
            }
        }
    }
}
