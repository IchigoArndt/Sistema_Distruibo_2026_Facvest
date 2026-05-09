using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Professionals;
using SD_Server.Domain.Features.Students;

namespace SD_Server.Domain.Features.Avaliations
{
    public class BodyComposition //Composição Corporal
    {
        public double? Weight { get; set; } //Peso
        public double? Height { get; set; } //Altura
        public double? Waist { get; set; } //Cintura
        public double? Abdomen { get; set; } //Abdômen
        public double? Hips { get; set; } //Quadril
        public double? Chest { get; set; } //Tórax
        public double? ArmRight { get; set; } //Braço Direito
        public double? ArmLeft { get; set; } //Braço Esquerdo
        public double? RightThigh { get; set; } //Coxa Direita
        public double? LeftThigh { get; set; } //Coxa Esquerda
    }

    public class Skinfolds
    {
        public double? Pectoral { get; set; }
        public double? MidAxillary { get; set; }
        public double? Triceps { get; set; }
        public double? Subscapular { get; set; }
        public double? Suprailiac { get; set; }
        public double? Abdominal { get; set; }
        public double? Thigh { get; set; }
    }

    public class Anamnesis
    {
        public bool HasInjury { get; set; }
        public string? InjuryDescription { get; set; }
        public bool HasMedication { get; set; }
        public string? MedicationDescription { get;set; }
        public ActivityLevelEnum ActivityLevel { get; set; }
    }

    public class Avaliation : BaseEntity
    {
        //FK Student
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        //FK Professional
        public int ProfessionalId { get; set; }
        public virtual Professional Professional { get; set; }

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
