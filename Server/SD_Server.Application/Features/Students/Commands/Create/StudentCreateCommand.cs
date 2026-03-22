using MediatR;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Students.Commands.Create
{
    public class StudentCreateCommand : IRequest<Result<Exception, Unit>>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string CellPhone { get; set; } = string.Empty;
    }
}
