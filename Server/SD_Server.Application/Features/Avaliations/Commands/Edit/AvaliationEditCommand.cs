using MediatR;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Avaliations;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Avaliations.Commands.Edit
{
    public class AvaliationEditCommand : IRequest<Result<Exception, Unit>>
    {
        public int Id { get; set; }
        public BodyComposition? BodyComposition { get; set; }
        public Skinfolds? Skinfolds { get; set; }
        public Anamnesis? Anamnesis { get; set; }
        public string? TechnicalOpinion { get; set; }
        public string? IMC { get; set; }
        public string? BodyFatPercentage { get; set; }
        public DateTime? DateNextAvaliation { get; set; }
        public StatusAssessmentEnum? Status { get; set; }
    }
}
