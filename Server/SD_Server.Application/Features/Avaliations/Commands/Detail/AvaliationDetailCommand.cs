using MediatR;
using SD_Server.Application.Features.Avaliations.DTO;
using SD_SharedKernel.Helpers;

namespace SD_Server.Application.Features.Avaliations.Commands.Detail
{
    public class AvaliationDetailCommand : IRequest<Result<Exception, AvaliationDTO>>
    {
        public int Id { get; set; }
    }
}
