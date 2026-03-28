using FluentValidation;

namespace SD_Server.Application.Features.Students.Commands.Detail
{
    public class StudentDetailCommandValidator : AbstractValidator<StudentDetailCommand>
    {
        public StudentDetailCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("O Identificador, não pode conter um valor menor que 0 ou ser vazio");
        }
    }
}
