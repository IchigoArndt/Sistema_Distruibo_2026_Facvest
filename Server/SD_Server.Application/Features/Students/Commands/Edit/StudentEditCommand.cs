using MediatR;
using SD_Server.Domain.Enum;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Students.Commands.Edit
{
    public class StudentEditCommand : IRequest<Result<Exception, Unit>>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
        public string? CellPhone { get; set; }
    }
}
