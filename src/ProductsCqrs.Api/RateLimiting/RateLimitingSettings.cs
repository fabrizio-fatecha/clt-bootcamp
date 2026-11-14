namespace ProductsCqrs.Api.RateLimiting;

public sealed class RateLimitingSettings
{
    public int ReadsPerDay { get; init; }
    public int WritesPerDay { get; init; }
}
