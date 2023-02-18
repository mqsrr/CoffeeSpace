
namespace CoffeeSpace.WebAPI.Extensions;

public static class AddApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService<TInterface>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        services.Scan(scan =>
            scan.FromAssemblyOf<Program>()
                .AddClasses(classes => classes.AssignableTo(typeof(TInterface)))
                .AsImplementedInterfaces()
                .WithLifetime(serviceLifetime));
        
        return services;
    }    
    
    public static IServiceCollection AddApplicationService(this IServiceCollection services, Type interfaceType,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        services.Scan(scan =>
            scan.FromAssemblyOf<Program>()
                .AddClasses(classes => classes.AssignableTo(interfaceType))
                .AsImplementedInterfaces()
                .WithLifetime(serviceLifetime));
        
        return services;
    }
}