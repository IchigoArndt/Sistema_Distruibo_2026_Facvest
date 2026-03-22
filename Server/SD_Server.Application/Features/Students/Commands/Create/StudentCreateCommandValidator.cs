using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD_Server.Application.Features.Students.Commands.Create
{
    public class StudentCreateCommandValidator : AbstractValidator<StudentCreateCommand>
    {
        public StudentCreateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do aluno é obrigatório.")
                .MaximumLength(50).WithMessage("O nome do aluno deve ter no máximo 50 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O Email do aluno é obrigatório")
                .EmailAddress().WithMessage("O Email do aluno deve ser um endereço de email válido.");

            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("É obrigatório informar a idade do aluno.")
                .GreaterThan(12).WithMessage("O aluno deve ter mais de 12 anos.");

            RuleFor(x => x.CellPhone)
                .NotEmpty().WithMessage("O número de celular do aluno é obrigatório.")
                .Matches(@"^\d{10,11}$").WithMessage("O número de celular deve conter apenas dígitos e ter entre 10 e 11 caracteres.");
        }
    }
}
