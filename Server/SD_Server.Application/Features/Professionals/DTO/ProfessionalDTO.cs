using SD_Server.Domain.Enum;

namespace SD_Server.Application.Features.Professionals.DTO
{
    public class ProfessionalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Cref { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public StatusEnum Status { get; set; }
    }
}
