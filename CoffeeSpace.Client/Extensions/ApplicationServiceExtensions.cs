
using System.Collections.Immutable;
using CoffeeSpace.Client.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CoffeeSpace.Client.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddPagesFromAssembly<TPageModel>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TPageModel>()
            .AddClasses()
            .AsSelf()
            .WithLifetime(serviceLifetime));

        return services;
    }
    
    public static IServiceCollection AddApplicationService<TInterface>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TInterface>()
            .AddClasses(classes => classes.AssignableTo<TInterface>())
            .AsImplementedInterfaces()
            .WithLifetime(serviceLifetime));

        return services;
    }

    public static IServiceCollection AddApiKeyOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddTransient(_ => Options.Create(new ApiKeySettings
        {
            ApiKey = configuration["Authorization:ApiKey"],
            HeaderName = configuration["Authorization:HeaderName"]
        }));

        return serviceCollection;
    }
}