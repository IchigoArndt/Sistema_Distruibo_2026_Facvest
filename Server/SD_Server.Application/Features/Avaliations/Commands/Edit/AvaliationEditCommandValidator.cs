using FluentValidation;

namespace SD_Server.Application.Features.Avaliations.Commands.Edit
{
    public class AvaliationEditCommandValidator : AbstractValidator<AvaliationEditCommand>
    {
        public AvaliationEditCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("O identificador da avaliação deve ser maior que zero.");
        }
    }
}
