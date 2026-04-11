using FluentValidation;

namespace SD_Server.Application.Helpers
{
    public static class PasswordValidationRules
    {
        public static IRuleBuilderOptions<T, string> ApplyPasswordRules<T>(
         this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("A senha é obrigatória")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres")
                .Must(p => p.Any(char.IsUpper)).WithMessage("A senha deve conter pelo menos uma letra maiúscula")
                .Must(p => p.Any(char.IsLower)).WithMessage("A senha deve conter pelo menos uma letra minúscula")
                .Must(NaoTerDigitosRepetidos).WithMessage("A senha não pode ter dígitos repetidos consecutivos");
        }
        private static bool NaoTerDigitosRepetidos(string password)
        {
            for (int i = 0; i < password.Length - 1; i++)
            {
                if (char.IsDigit(password[i]) && password[i] == password[i + 1])
                    return false;
            }
            return true;
        }
    }
}
