using MediatR;
using SD_SharedKernel.Helpers;
using System;

namespace SD_Server.Application.Features.Assessments.Commands.Create
{
    public class AssessmentCreateCommand : IRequest<Result<Exception, SD_Server.Application.Features.Assessments.DTO.AssessmentDTO>>
    {
        public int StudentId { get; set; }
        public int ProfessionalId { get; set; }
        public DateTime Date { get; set; }
        public SD_Server.Domain.Enum.StatusEnum Status { get; set; }
        public string Methodology { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Notes { get; set; }
        public string? Results { get; set; }
    }
}
