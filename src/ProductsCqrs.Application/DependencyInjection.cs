using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductsCqrs.Application.Abstractions.Messaging;
using ProductsCqrs.Application.Behaviors;
using ProductsCqrs.Application.Messaging;

namespace ProductsCqrs.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddScoped<ISender, Sender>();

        AddRequestHandlers(services, assembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        return services;
    }

    private static void AddRequestHandlers(IServiceCollection services, Assembly assembly)
    {
        var handlerRegistrations = assembly.GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false, IsGenericTypeDefinition: false })
            .SelectMany(type => type.GetInterfaces()
                .Where(contract => contract.IsGenericType && contract.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .Select(contract => (Contract: contract, Implementation: type)));

        foreach (var (contract, implementation) in handlerRegistrations)
            services.AddScoped(contract, implementation);
    }
}
