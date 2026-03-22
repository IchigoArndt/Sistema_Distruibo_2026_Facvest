using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;

namespace SD_Server.Domain.Features.Students
{
    public class Student : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string CellPhone { get; set; }
        public StatusEnum Status { get; set; } = StatusEnum.Active;
        public DateTime LastReview { get; set; }

    }
}
