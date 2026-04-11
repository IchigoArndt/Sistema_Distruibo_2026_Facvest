using FluentValidation;
using SD_Server.Application.Helpers;

namespace SD_Server.Application.Features.Students.Commands.Edit
{
    public class StudentEditCommandValidator : AbstractValidator<StudentEditCommand>
    {
        public StudentEditCommandValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(50).WithMessage("O nome do aluno deve ter no máximo 50 caracteres.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("O Email do aluno deve ser um endereço de email válido.");

            RuleFor(x => x.Age)
                .GreaterThan(12).WithMessage("O aluno deve ter mais de 12 anos.");

            RuleFor(x => x.CellPhone)
                .Matches(@"^\d{10,11}$").WithMessage("O número de celular deve conter apenas dígitos e ter entre 10 e 11 caracteres.");

            When(x => !string.IsNullOrEmpty(x.Password), () =>
            {
                RuleFor(x => x.Password).ApplyPasswordRules();
            });
        }
    }
}
