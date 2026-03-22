using FluentValidation;
using SD_SharedKernel.Helpers;
using Unit = SD_SharedKernel.Helpers.Unit;

namespace SD_Server.Application.Helpers
{
    public static class ValidationHelper<T,X> where T : AbstractValidator<X> //Aqui é o tipo do validador, ou seja, a classe que contém as regras de validação para o comando ou entidade
                                              where X : class //Aqui é a classe que será validada, ou seja, o comando ou a entidade que queremos validar
    {
            public static Result<Exception, Unit> Validate(T validator, X instance)
            {
                var validationResult = validator.Validate(instance);
    
                if (!validationResult.IsValid)
                {
                    var error = validationResult.Errors.FirstOrDefault();
    
                    return new Exception(error.ErrorMessage);
                }
    
                return Unit.Sucessful;
        }
    }
}
