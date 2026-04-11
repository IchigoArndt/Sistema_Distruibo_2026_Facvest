using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SD_Server.Auth.Commands;
using SD_Server.Auth.DTO;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Users;
using SD_SharedKernel.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace SD_Server.Auth.Handlers
{
    public class AuthLogin
    {
        public class Handler(ILogger<Handler> logger, IUserRepository userRepository, IConfiguration configuration) : IRequestHandler<LoginCommand, Result<Exception, LoginDTO>>
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

                var userResult = await userRepository.GetByEmail(request.Email);

                if (userResult.IsFailure)
                {
                    logger.LogError("Erro ao buscar o usuário: {Error}", userResult.Failure.Message);
                    return new Exception("Email ou senha inválidos.");
                }

                var validPassword = BC.Verify(request.Password, userResult.Success.PasswordHash);

                if (!validPassword)
                {
                    logger.LogError("Senha informada não confere com o hash salvo no banco");
                    return new Exception("Email ou senha inválidos.");
                }

                var tokenResult = GenerateToken(userResult.Success);

                if (tokenResult.IsFailure)
                    return new Exception("Erro ao gerar o token de autenticação.");

                var expiration = DateTime.UtcNow.AddMinutes(30);

                return new LoginDTO
                {
                    Token      = tokenResult.Success,
                    Expiration = expiration.ToString("o")
                };
            }

            private Result<Exception, string> GenerateToken(User user)
            {
                var secret = Environment.GetEnvironmentVariable("Secret")
                    ?? configuration["AppSettings:JwtSecret"]
                    ?? throw new InvalidOperationException("Secret JWT não configurado.");

                var key          = Encoding.UTF8.GetBytes(secret);
                var tokenHandler = new JwtSecurityTokenHandler();

                var claimList = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, GetRole(user.TypeAccess))
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer             = "MatrixCompetency",
                    Audience           = "MatrixCompetency",
                    Subject            = new ClaimsIdentity(claimList),
                    Expires            = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            private string GetRole(TypeUserEnum acess) => acess switch
            {
                TypeUserEnum.Admin        => "Admin",
                TypeUserEnum.Professional => "Professional",
                TypeUserEnum.Student      => "Student",
                _                         => "Admin"
            };
        }
    }
}
