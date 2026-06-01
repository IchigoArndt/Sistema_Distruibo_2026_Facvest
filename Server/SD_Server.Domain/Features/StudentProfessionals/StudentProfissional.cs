using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Professionals;
using SD_Server.Domain.Features.Students;

namespace SD_Server.Domain.Features.StudentProfessionals;

public class StudentProfessional : BaseEntity
{
    public int StudentId { get; set; }
    public virtual Student Student { get; set; }
    public int ProfessionalId { get; set; }
    public virtual Professional Professional { get; set; }
    public DateTime LinkedAt { get; set; } = DateTime.UtcNow;
    public StatusEnum Status { get; set; } = StatusEnum.Active;
}