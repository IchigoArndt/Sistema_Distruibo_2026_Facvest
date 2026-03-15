using MediatR;
using Microsoft.IdentityModel.Tokens;
using SD_Server.Auth.Commands;
using SD_Server.Auth.Domain;
using SD_Server.Auth.DTO;
using SD_SharedKernel.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SD_Server.Auth.Handlers
{
    public class AuthLogin
    {
        public class Handler(ILogger<Handler> logger, IMediator mediator) : IRequestHandler<LoginCommand, Result<Exception, LoginDTO>>
        {
            public async Task<Result<Exception, LoginDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var validator = new loginCommandValidator();

                var validatorResult = validator.Validate(request);

                if (!validatorResult.IsValid)
                {
                    var errorMessage = validatorResult.Errors.FirstOrDefault()?.ErrorMessage;

                    return new Exception(errorMessage);
                }

                //Aqui virá um busca no banco pelo email do usuário, e depois uma comparação da senha, e se tudo estiver certo, a geração do token JWT

                var passwordValid = false;

                if (!request.Email.Contains("admin"))
                    return new Exception("Não implmentado ainda !_! volte mais tarde XD.");
                else
                    passwordValid = true;


                var tokenResult = await GenerateToken(request.Email);

                if (tokenResult.IsFailure)
                {
                    return new Exception("Erro ao gerar o token de autenticação.");
                }

                return new LoginDTO
                {
                    Token = tokenResult.Success,
                    Expiration = DateTime.UtcNow.AddMinutes(30).Millisecond.ToString()
                };
            }

            private async Task<Result<Exception, string>> GenerateToken(string email)
            {
                //Nesse metodo como parametro deverá vir um usuario que foi buscado e encontrado no banco, e depois disso,
                //a geração do token JWT com as informações do usuário, como id, email, nome e acesso.

                var tokenHandler = new JwtSecurityTokenHandler();

                var secret = Environment.GetEnvironmentVariable("Secret") ?? throw new InvalidOperationException("Secret não configurado.");

                var key = Encoding.UTF8.GetBytes(secret);

                var claimList = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1"),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Name, "Admin"),
                        new Claim(ClaimTypes.Role, GetRole(TypeAcess.admin))
                    };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = "MatrixCompetency",
                    Audience = "MatrixCompetency",
                    Subject = new ClaimsIdentity(claimList),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }

            private string GetRole(TypeAcess acess)
            {
                //Esse metodo é para converter o enum de acesso do usuário para uma string que será usada como role no token JWT, e depois disso, na autorização das rotas.
                switch (acess)
                {
                    case TypeAcess.admin:
                        return "Admin";
                    default:
                        return "Admin";
                }
            }
        }
    }
}
