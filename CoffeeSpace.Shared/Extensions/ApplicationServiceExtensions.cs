using CoffeeSpace.Shared.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.Shared.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationService<TInterface>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TInterface>()
            .AddClasses(classes =>
            {
                classes.AssignableTo<TInterface>()
                    .WithoutAttribute<Decorator>();
            })
            .AsImplementedInterfaces()
            .WithLifetime(serviceLifetime));

        return services;
    }

    public static IServiceCollection AddApplicationServiceAsSelf<TImplementation>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TImplementation>()
            .AddClasses(classes =>
            {
                classes.AssignableTo<TImplementation>()
                    .WithoutAttribute<Decorator>();
            })
            .AsSelfWithInterfaces()
            .WithLifetime(serviceLifetime));

        return services;
    }

    public static IServiceCollection AddApplicationService(
        this IServiceCollection services,
        Type interfaceType,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Scan(scan => scan
            .FromAssembliesOf(interfaceType)
            .AddClasses(classes =>
            {
                classes.AssignableTo(interfaceType)
                    .WithoutAttribute<Decorator>();
            })
            .AsImplementedInterfaces()
            .WithLifetime(serviceLifetime));

        return services;
    }

    public static IServiceCollection AddApplicationService(
        this IServiceCollection services,
        Type interfaceType,
        Type assemblyOf,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Scan(scan => scan
            .FromAssembliesOf(assemblyOf)
            .AddClasses(classes =>
            {
                classes.AssignableTo(interfaceType)
                    .WithoutAttribute<Decorator>();
            })
            .AsImplementedInterfaces()
            .WithLifetime(serviceLifetime));

        return services;
    }
}