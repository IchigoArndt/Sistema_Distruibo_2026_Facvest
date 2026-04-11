using MediatR;
using SD_Server.Auth.Domain;
using SD_Server.Auth.DTO;
using SD_SharedKernel.Helpers;

namespace SD_Server.Auth.Commands
{
    public class LoginCommand : IRequest<Result<Exception, LoginDTO>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
