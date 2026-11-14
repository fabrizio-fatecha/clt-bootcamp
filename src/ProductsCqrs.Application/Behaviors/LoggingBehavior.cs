using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ProductsCqrs.Application.Abstractions.Messaging;

namespace ProductsCqrs.Application.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        var startedAt = Stopwatch.GetTimestamp();

        try
        {
            var response = await next();
            logger.LogInformation("{Request} completado en {Milliseconds:N1} ms", requestName, ElapsedMilliseconds(startedAt));
            return response;
        }
        catch (Exception exception)
        {
            logger.LogWarning("{Request} falló en {Milliseconds:N1} ms: {Error}", requestName, ElapsedMilliseconds(startedAt), exception.Message);
            throw;
        }
    }

    private static double ElapsedMilliseconds(long startedAt)
        => Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds;
}
