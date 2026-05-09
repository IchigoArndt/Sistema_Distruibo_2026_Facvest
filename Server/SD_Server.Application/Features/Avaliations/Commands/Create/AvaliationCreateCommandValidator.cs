using FluentValidation;
namespace SD_Server.Application.Features.Avaliations.Commands.Create
{
    public class AvaliationCreateCommandValidator : AbstractValidator<AvaliationCreateCommand>
    {
        public AvaliationCreateCommandValidator()
        {
            RuleFor(x => x.StudentId).GreaterThan(0).WithMessage("O Id do aluno deve ser maior que zero.");
            RuleFor(x => x.ProfessionalId).GreaterThan(0).WithMessage("O Id do Profissional deve ser maior que zero.");
            RuleFor(x => x.Date).NotEmpty().WithMessage("A data da avaliação é obrigatória.");
            RuleFor(x => x.StudentObjective).NotEmpty().IsInEnum().WithMessage("O objetivo do aluno é inválido.");
            RuleFor(x => x.Status).NotEmpty().IsInEnum().WithMessage("O status da avaliação é inválido.");
        }
    }
}
