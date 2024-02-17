using Common.Responses.Wrappers;
using FluentValidation;
using MediatR;

namespace Application.Pipelines
{
    public class ValidatePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, IValidateMe
    {
        private readonly IEnumerable<IValidator<TResponse>> _validators;

        public ValidatePipelineBehavior(IEnumerable<IValidator<TResponse>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task
                    .WhenAll(_validators
                    .Select(validator => validator.ValidateAsync(context, cancellationToken)));

                if (!validationResults.Any(validator => validator.IsValid))
                {
                    List<string> errors = [];
                    var failures = validationResults.SelectMany(validator => validator.Errors)
                        .Where(f => f != null)
                        .ToList();
                    foreach (var failure in failures)
                    {
                        errors.Add(failure.ErrorMessage);
                    }
                    return (TResponse)await ResponseWrapper.FailAsync(errors);
                }
            }
            return await next();
        }
    }
}
