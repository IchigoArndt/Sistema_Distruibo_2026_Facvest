using FluentValidation;

namespace SD_Server.Auth.Commands
{
    public class loginCommandValidator : AbstractValidator<LoginCommand>
    {
        public loginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Por favor digite o seu e-mail")
                .Length(6, 100).WithMessage("O e-mail informado não pode conter mais que 100 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Por favor digite a sua senha")
                .Length(6, 15).WithMessage("A senha informada não pode conter mais que 15 e nem menos que 6 caracteres");
        }
    }
}
