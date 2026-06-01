using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Avaliations;

namespace SD_Server.Application.Features.Avaliations.DTO
{
    public class AvaliationDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int ProfessionalId { get; set; }
        public string? ProfessionalName { get; set; }
        public DateTime Date { get; set; }
        public TypeAvaliationEnum TypeAvaliation { get; set; }
        public StudentObjectiveEnum StudentObjective { get; set; }
        public StatusAssessmentEnum Status { get; set; }
        public BodyComposition? BodyComposition { get; set; }
        public Skinfolds? Skinfolds { get; set; }
        public Anamnesis? Anamnesis { get; set; }
        public string? TechnicalOpinion { get; set; }
        public DateTime? DateNextAvaliation { get; set; }
        public string? IMC { get; set; }
        public string? BodyFatPercentage { get; set; }
    }
}
