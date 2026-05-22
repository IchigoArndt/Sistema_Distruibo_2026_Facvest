using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;
using System;

namespace SD_Server.Domain.Features.Assessments
{
    public class Assessment : BaseEntity
    {
        public int StudentId { get; set; }
        public int ProfessionalId { get; set; }
        public DateTime Date { get; set; }
        public StatusEnum Status { get; set; }
        public string Methodology { get; set; }
        public decimal Price { get; set; }
        public string? Notes { get; set; }
        public string? Results { get; set; }
    }
}
