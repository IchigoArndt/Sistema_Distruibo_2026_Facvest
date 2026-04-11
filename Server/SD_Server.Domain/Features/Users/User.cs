using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;

namespace SD_Server.Domain.Features.Users
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public TypeUserEnum TypeAccess { get; set; }
        public StatusEnum Status { get; set; } = StatusEnum.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? EntityId { get; set; } // FK para Student, Professional ou Admin
    }
}
