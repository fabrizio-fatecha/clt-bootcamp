using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using ProductsCqrs.Application.Abstractions.Data;
using ProductsCqrs.Domain.Common;
using ProductsCqrs.Domain.Products;
using ProductsCqrs.Infrastructure.Persistence;
using ProductsCqrs.Infrastructure.ReadModels;

namespace ProductsCqrs.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = BuildConnectionString(configuration);

        services.AddDbContext<ProductsDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ProductsDbContext>());
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IProductReadStore, ProductReadStore>();

        services.AddSingleton(TimeProvider.System);

        return services;
    }

    private static string BuildConnectionString(IConfiguration configuration)
        => new NpgsqlConnectionStringBuilder
        {
            Host = configuration["POSTGRES_HOST"] ?? "localhost",
            Port = int.Parse(configuration["POSTGRES_PORT"] ?? "5432"),
            Database = configuration.GetRequiredValue("POSTGRES_DB"),
            Username = configuration.GetRequiredValue("POSTGRES_USER"),
            Password = configuration.GetRequiredValue("POSTGRES_PASSWORD"),
            GssEncryptionMode = GssEncryptionMode.Disable
        }.ConnectionString;
}
