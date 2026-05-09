using FluentValidation;

namespace SD_Server.Application.Features.Avaliations.Commands.Detail
{
    public class AvaliationDetailCommandValidator : AbstractValidator<AvaliationDetailCommand>
    {
        public AvaliationDetailCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("O identificador da avaliação não pode ser vazio ou menor que zero.");
        }
    }
}
