using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SD_Server.Auth.Commands;
using SD_Server.Auth.Handlers;
using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Users;
using SD_SharedKernel.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BC = BCrypt.Net.BCrypt;

namespace SD_Server.Auth.Tests.Handlers
{
    public class AuthLoginHandlerTests
    {
        private readonly Mock<ILogger<AuthLogin.Handler>> _loggerMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IConfiguration _configuration;
        private readonly AuthLogin.Handler _handler;

        private const string ValidSecret = "SD_Server_Secret_Key_2026_Facvest_MatrixCompetency_32chars!";

        public AuthLoginHandlerTests()
        {
            _loggerMock          = new Mock<ILogger<AuthLogin.Handler>>();
            _userRepositoryMock  = new Mock<IUserRepository>();

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["AppSettings:JwtSecret"] = ValidSecret
                })
                .Build();

            _handler = new AuthLogin.Handler(_loggerMock.Object, _userRepositoryMock.Object, _configuration);
        }

        // ─── Cenários de Validação ───────────────────────────────────────────────

        [Fact]
        public async Task Handle_EmailVazio_DeveRetornarFalha()
        {
            var command = new LoginCommand { Email = "", Password = "Senha@1" };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Failure.Message.Should().Contain("e-mail");
        }

        [Fact]
        public async Task Handle_SenhaVazia_DeveRetornarFalha()
        {
            var command = new LoginCommand { Email = "usuario@teste.com", Password = "" };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Failure.Message.Should().Contain("senha");
        }

        [Fact]
        public async Task Handle_SenhaMenorQue6Caracteres_DeveRetornarFalha()
        {
            var command = new LoginCommand { Email = "usuario@teste.com", Password = "Ab1" };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_EmailInvalido_DeveRetornarFalha()
        {
            // O validator rejeita emails com menos de 6 caracteres; "a@b.c" tem 5 chars
            var command = new LoginCommand { Email = "a@b.c", Password = "Senha@1" };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            _userRepositoryMock.Verify(r => r.GetByEmail(It.IsAny<string>()), Times.Never);
        }

        // ─── Cenários de Autenticação ────────────────────────────────────────────

        [Fact]
        public async Task Handle_EmailNaoEncontrado_DeveRetornarFalha()
        {
            var command = BuildValidCommand();

            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                .ReturnsAsync(Result<Exception, User>.Of(new Exception("Usuário não encontrado")));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Failure.Message.Should().Be("Email ou senha inválidos.");
        }

        [Fact]
        public async Task Handle_SenhaIncorreta_DeveRetornarFalha()
        {
            var command = BuildValidCommand();
            var user    = BuildUser(TypeUserEnum.Admin, senhaCorreta: "OutraSenha@9");

            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                .ReturnsAsync(Result<Exception, User>.Of(user));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Failure.Message.Should().Be("Email ou senha inválidos.");
        }

        [Fact]
        public async Task Handle_CredenciaisValidas_DeveRetornarToken()
        {
            var command = BuildValidCommand();
            var user    = BuildUser(TypeUserEnum.Admin, senhaCorreta: command.Password);

            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                .ReturnsAsync(Result<Exception, User>.Of(user));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Success.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_CredenciaisValidas_TokenContemClaimsCorretas()
        {
            var command = BuildValidCommand();
            var user    = BuildUser(TypeUserEnum.Admin, senhaCorreta: command.Password);

            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                .ReturnsAsync(Result<Exception, User>.Of(user));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(result.Success.Token);

            // JwtSecurityTokenHandler usa short claim names ao ler o token
            jwtToken.Claims.Should().Contain(c => c.Type == "email"       && c.Value == user.Email);
            jwtToken.Claims.Should().Contain(c => c.Type == "unique_name" && c.Value == user.Name);
            jwtToken.Claims.Should().Contain(c => c.Type == "role"        && c.Value == "Admin");
            jwtToken.Issuer.Should().Be("MatrixCompetency");
        }

        [Fact]
        public async Task Handle_CredenciaisValidas_ExpiracaoPreenchida()
        {
            var command = BuildValidCommand();
            var user    = BuildUser(TypeUserEnum.Admin, senhaCorreta: command.Password);

            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                .ReturnsAsync(Result<Exception, User>.Of(user));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Success.Expiration.Should().NotBeNullOrEmpty();

            var expiration = DateTime.Parse(result.Success.Expiration, null, System.Globalization.DateTimeStyles.RoundtripKind);
            expiration.ToUniversalTime().Should().BeAfter(DateTime.UtcNow);
        }

        // ─── Cenários de Role ────────────────────────────────────────────────────

        [Theory]
        [InlineData(TypeUserEnum.Admin,        "Admin")]
        [InlineData(TypeUserEnum.Professional, "Professional")]
        [InlineData(TypeUserEnum.Student,      "Student")]
        public async Task Handle_CredenciaisValidas_RoleCorretaNoToken(TypeUserEnum tipoUsuario, string roleEsperada)
        {
            var command = BuildValidCommand();
            var user    = BuildUser(tipoUsuario, senhaCorreta: command.Password);

            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                .ReturnsAsync(Result<Exception, User>.Of(user));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(result.Success.Token);
            jwtToken.Claims.Should().Contain(c => c.Type == "role" && c.Value == roleEsperada);
        }

        // ─── Helpers ─────────────────────────────────────────────────────────────

        private static LoginCommand BuildValidCommand() => new()
        {
            Email    = "usuario@teste.com",
            Password = "Senha@1Valida"
        };

        private static User BuildUser(TypeUserEnum tipo, string senhaCorreta) => new()
        {
            Id           = 1,
            Name         = "Usuário Teste",
            Email        = "usuario@teste.com",
            PasswordHash = BC.HashPassword(senhaCorreta),
            TypeAccess   = tipo,
            Status       = SD_Server.Domain.Enum.StatusEnum.Active
        };
    }
}
