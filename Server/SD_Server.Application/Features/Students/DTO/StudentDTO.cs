using SD_Server.Domain.Enum;

namespace SD_Server.Application.Features.Students.DTO
{
    public class StudentDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string CellPhone { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime LastReview { get; set; }
    }
}
