using FluentValidation;
using ProductsCqrs.Application.Abstractions.Messaging;

namespace ProductsCqrs.Application.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(context, ct)));
        var failures = results.SelectMany(result => result.Errors).ToList();

        if (failures.Count > 0)
        {
            var details = string.Join(" ", failures.Select(failure => failure.ErrorMessage));
            throw new ValidationException($"La solicitud no superó la validación. {details}", failures);
        }

        return await next();
    }
}
