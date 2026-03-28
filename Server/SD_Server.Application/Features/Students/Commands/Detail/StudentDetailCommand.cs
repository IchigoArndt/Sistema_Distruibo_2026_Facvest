using MediatR;
using SD_Server.Application.Features.Students.DTO;
using SD_SharedKernel.Helpers;

namespace SD_Server.Application.Features.Students.Commands.Detail
{
    public class StudentDetailCommand : IRequest<Result<Exception, StudentDTO>>
    {
        public int Id { get; set; }
    }
}
