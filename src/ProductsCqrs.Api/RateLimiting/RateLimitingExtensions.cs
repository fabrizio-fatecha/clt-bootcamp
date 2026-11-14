using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ProductsCqrs.Infrastructure;

namespace ProductsCqrs.Api.RateLimiting;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddIpRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new RateLimitingSettings
        {
            ReadsPerDay = configuration.GetRequiredNumber("RATE_LIMIT_READS_PER_DAY"),
            WritesPerDay = configuration.GetRequiredNumber("RATE_LIMIT_WRITES_PER_DAY")
        };

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(RateLimitPolicies.Reads, httpContext => LimitPerIp(httpContext, settings.ReadsPerDay));
            options.AddPolicy(RateLimitPolicies.Writes, httpContext => LimitPerIp(httpContext, settings.WritesPerDay));
            options.OnRejected = (context, ct) => WriteTooManyRequests(context, settings, ct);
        });

        return services;
    }

    private static RateLimitPartition<string> LimitPerIp(HttpContext httpContext, int requestsPerDay)
    {
        var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = requestsPerDay,
            Window = TimeSpan.FromDays(1)
        });
    }

    private static async ValueTask WriteTooManyRequests(OnRejectedContext context, RateLimitingSettings settings, CancellationToken ct)
    {
        var httpContext = context.HttpContext;

        httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            httpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString(CultureInfo.InvariantCulture);

        var detail = HttpMethods.IsGet(httpContext.Request.Method)
            ? $"Se alcanzó el límite diario de lecturas para su dirección IP. Máximo por día: {settings.ReadsPerDay}. Intente nuevamente más tarde."
            : $"Se alcanzó el límite diario de escrituras (crear, actualizar o eliminar) para su dirección IP. Máximo por día: {settings.WritesPerDay}. Intente nuevamente más tarde.";

        var problemDetailsService = httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status429TooManyRequests,
                Title = "Demasiadas solicitudes",
                Detail = detail
            }
        });
    }
}
