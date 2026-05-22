using MediatR;
using Microsoft.Extensions.Logging;
using SD_SharedKernel.Helpers;
using SD_Server.Application.Features.Assessments.DTO;
using SD_Server.Domain.Features.Assessments;
using System.Linq;

namespace SD_Server.Application.Features.Assessments.Handlers
{
    public class AssessmentCollectionHandler
    {
        public class Query : IRequest<Result<Exception, IQueryable<AssessmentDTO>>> { }

        public class Handler(ILogger<Handler> logger, IAssessmentRepository repository) : IRequestHandler<Query, Result<Exception, IQueryable<AssessmentDTO>>>
        {
            public async Task<Result<Exception, IQueryable<AssessmentDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await repository.GetAllAsync();
                if (result.IsFailure) return result.Failure;

                var collection = result.Success.Select(x => new AssessmentDTO
                {
                    Id = x.Id,
                    StudentId = x.StudentId,
                    ProfessionalId = x.ProfessionalId,
                    Date = x.Date,
                    Status = x.Status,
                    Methodology = x.Methodology,
                    Price = x.Price,
                    Notes = x.Notes,
                    Results = x.Results
                }).AsQueryable();

                return Result<Exception, IQueryable<AssessmentDTO>>.Of(collection);
            }
        }
    }
}
