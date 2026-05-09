using FluentValidation;

namespace SD_Server.Application.Features.Avaliations.Commands.Delete
{
    public class AvaliationDeleteCommandValidator : AbstractValidator<AvaliationDeleteCommand>
    {
        public AvaliationDeleteCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("O identificador da avaliação não pode ser vazio ou menor que zero.");
        }
    }
}
