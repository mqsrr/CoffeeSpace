
using System.Reflection;

namespace CoffeeSpace.WebAPI.Extensions;

public static class AddApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService<TInterface>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        services.Scan(scan =>
            scan.FromAssemblyDependencies(typeof(TInterface).Assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(TInterface)))
                .AsImplementedInterfaces()
                .WithLifetime(serviceLifetime));
        
        return services;
    }    
    
    public static IServiceCollection AddApplicationService(this IServiceCollection services, Type interfaceType, Assembly? assembly,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        assembly ??= interfaceType.Assembly;
        
        services.Scan(scan =>
            scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(interfaceType))
                .AsImplementedInterfaces()
                .WithLifetime(serviceLifetime));
        
        return services;
    }
}