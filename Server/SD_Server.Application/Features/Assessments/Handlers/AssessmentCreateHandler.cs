using MediatR;
using Microsoft.Extensions.Logging;
using SD_Server.Application.Features.Assessments.Commands.Create;
using SD_Server.Application.Features.Assessments.DTO;
using SD_Server.Domain.Features.Assessments;
using SD_SharedKernel.Helpers;
using SD_Server.Domain.Base;

namespace SD_Server.Application.Features.Assessments.Handlers
{
    public class AssessmentCreateHandler
    {
        public class Handler(ILogger<Handler> logger, IAssessmentRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<AssessmentCreateCommand, Result<Exception, AssessmentDTO>>
        {
            public async Task<Result<Exception, AssessmentDTO>> Handle(AssessmentCreateCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Creating assessment for student {StudentId}", request.StudentId);

                await unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    var assessment = new Assessment
                    {
                        StudentId = request.StudentId,
                        ProfessionalId = request.ProfessionalId,
                        Date = request.Date,
                        Status = request.Status,
                        Methodology = request.Methodology,
                        Price = request.Price,
                        Notes = request.Notes,
                        Results = request.Results
                    };

                    var createResult = await repository.AddAsync(assessment);

                    if (createResult.IsFailure)
                    {
                        await unitOfWork.RollbackAsync(cancellationToken);
                        return createResult.Failure;
                    }

                    await unitOfWork.CommitAsync(cancellationToken);

                    var dto = new AssessmentDTO
                    {
                        Id = createResult.Success,
                        StudentId = assessment.StudentId,
                        ProfessionalId = assessment.ProfessionalId,
                        Date = assessment.Date,
                        Status = assessment.Status,
                        Methodology = assessment.Methodology,
                        Price = assessment.Price,
                        Notes = assessment.Notes,
                        Results = assessment.Results
                    };

                    return dto;
                }
                catch (Exception ex)
                {
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return ex;
                }
            }
        }
    }
}
