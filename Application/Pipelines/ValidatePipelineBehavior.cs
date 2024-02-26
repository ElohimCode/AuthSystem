using Common.Responses.Wrappers;
using FluentValidation;
using MediatR;

namespace Application.Pipelines
{
    public class ValidatePipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
       

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                List<string> errors = [];
                var validationResults = await Task
                    .WhenAll(validators
                    .Select(validator => validator.ValidateAsync(context, cancellationToken)));

                var failures = validationResults.SelectMany(r => r.Errors)
                  .Where(f => f != null)
                  .ToList();

                if (failures.Count != 0)
                {
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
