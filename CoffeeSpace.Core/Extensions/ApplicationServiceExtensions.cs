using CoffeeSpace.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.Core.Extensions;

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