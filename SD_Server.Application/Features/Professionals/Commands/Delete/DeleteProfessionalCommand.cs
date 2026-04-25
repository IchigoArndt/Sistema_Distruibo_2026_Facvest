using MediatR;

namespace SD_Server.Application.Features.Professionals.Commands.Delete
{
    public class DeleteProfessionalCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
