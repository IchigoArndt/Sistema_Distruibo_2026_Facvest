using FluentValidation;

namespace SD_Server.Application.Features.Professionals.Commands.Create
{
    public class CreateProfessionalCommandValidator : AbstractValidator<CreateProfessionalCommand>
    {
        public CreateProfessionalCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("O nome do profissional é obrigatório.").MaximumLength(50);

            RuleFor(x => x.Email).NotEmpty().WithMessage("O Email do profissional é obrigatório").EmailAddress().WithMessage("Email inválido.");

            RuleFor(x => x.Cref).NotEmpty().WithMessage("O CREF do profissional é obrigatório.");

            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.");
        }
    }
}
