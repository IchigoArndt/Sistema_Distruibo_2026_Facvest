using MediatR;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Features.Avaliations.Commands.Delete
{
    public class AvaliationDeleteCommand : IRequest<Result<Exception, Unit>>
    {
        public int Id { get; set; }
    }
}
