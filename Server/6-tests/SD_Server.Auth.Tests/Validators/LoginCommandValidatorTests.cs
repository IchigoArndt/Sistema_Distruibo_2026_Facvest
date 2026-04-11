using FluentAssertions;
using SD_Server.Auth.Commands;

namespace SD_Server.Auth.Tests.Validators
{
    public class LoginCommandValidatorTests
    {
        private readonly loginCommandValidator _validator = new();

        // ─── Email ───────────────────────────────────────────────────────────────

        [Fact]
        public void Validar_EmailVazio_DeveRetornarErro()
        {
            var command = new LoginCommand { Email = "", Password = "Senha@1" };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        public void Validar_EmailNulo_DeveRetornarErro()
        {
            var command = new LoginCommand { Email = null!, Password = "Senha@1" };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        public void Validar_EmailMuitoCurto_DeveRetornarErro()
        {
            var command = new LoginCommand { Email = "a@b", Password = "Senha@1" };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        public void Validar_EmailMuitoLongo_DeveRetornarErro()
        {
            // 91 'a' + "@teste.com" = 101 chars, ultrapassa o limite de 100
            var emailLongo = new string('a', 91) + "@teste.com";
            var command    = new LoginCommand { Email = emailLongo, Password = "Senha@1" };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        public void Validar_EmailValido_NaoDeveRetornarErroDeEmail()
        {
            var command = new LoginCommand { Email = "usuario@teste.com", Password = "Senha@1" };

            var result = _validator.Validate(command);

            result.Errors.Should().NotContain(e => e.PropertyName == "Email");
        }

        // ─── Senha ───────────────────────────────────────────────────────────────

        [Fact]
        public void Validar_SenhaVazia_DeveRetornarErro()
        {
            var command = new LoginCommand { Email = "usuario@teste.com", Password = "" };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Fact]
        public void Validar_SenhaNula_DeveRetornarErro()
        {
            var command = new LoginCommand { Email = "usuario@teste.com", Password = null! };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Fact]
        public void Validar_SenhaMenorQue6Caracteres_DeveRetornarErro()
        {
            var command = new LoginCommand { Email = "usuario@teste.com", Password = "Ab1" };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Fact]
        public void Validar_SenhaMaiorQue15Caracteres_DeveRetornarErro()
        {
            var command = new LoginCommand { Email = "usuario@teste.com", Password = "SenhaGrandeDemais123" };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Theory]
        [InlineData("Senha@1")]
        [InlineData("Ab1234")]
        [InlineData("Teste@99")]
        public void Validar_SenhaValida_NaoDeveRetornarErroDeSenha(string senha)
        {
            var command = new LoginCommand { Email = "usuario@teste.com", Password = senha };

            var result = _validator.Validate(command);

            result.Errors.Should().NotContain(e => e.PropertyName == "Password");
        }

        // ─── Comando completo ────────────────────────────────────────────────────

        [Fact]
        public void Validar_ComandoValido_DevePassar()
        {
            var command = new LoginCommand { Email = "usuario@teste.com", Password = "Senha@1" };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validar_ComandoInvalido_MensagemDeErroPreenchida()
        {
            var command = new LoginCommand { Email = "", Password = "" };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(1);
            result.Errors.All(e => !string.IsNullOrEmpty(e.ErrorMessage)).Should().BeTrue();
        }
    }
}
