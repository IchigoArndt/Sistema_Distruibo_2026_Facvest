using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;

namespace SD_Server.Domain.Features.Professionals
{
    public class Professional : BaseEntity
    {
        public string Name { get; set; } 
        public string Email { get; set; } 
        public string Phone { get; set; }
        public string Cref { get; set; } 
        public string? Bio { get; set; } 
        public string PasswordHash { get; set; } 
        public StatusEnum Status { get; set; }
    }
}