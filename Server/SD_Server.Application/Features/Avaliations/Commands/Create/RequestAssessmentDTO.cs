using SD_Server.Domain.Enum;

namespace SD_Server.Application.Features.Avaliations.Commands.Create
{
    public class RequestAssessmentDTO
    {
        public DateTime Date { get; set; }
        public TypeAvaliationEnum TypeAvaliation { get; set; }
        public StudentObjectiveEnum StudentObjective { get; set; }
    }
}
