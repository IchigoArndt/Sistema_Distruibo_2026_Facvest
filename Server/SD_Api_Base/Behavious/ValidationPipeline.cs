using FluentValidation;
using MediatR;
using SD_SharedKernel.Helpers;

namespace SD_Api_Base.Behavious
{
    public class ValidationPipeline<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, Result<Exception, TResponse>>
                where TRequest : IRequest<Result<Exception, TResponse>>
    {
        public async Task<Result<Exception, TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<Exception, TResponse>> next, CancellationToken cancellationToken)
        {
            var failures = validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Count != 0)
                return new ValidationException(failures);

            return await next();
        }
    }
}
