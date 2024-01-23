using Avalonia.Controls;
using CoffeeSpace.AClient.ViewModels;
using CoffeeSpace.AClient.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.AClient.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection RegisterViews(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.Scan(selector =>
            selector.FromAssemblyOf<MainWindow>()
                .AddClasses()
                .AsSelf()
                .WithLifetime(lifetime));

        return services;
    }
    
    internal static IServiceCollection RegisterViewModels(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.Scan(selector =>
            selector.FromAssemblyOf<MainWindowViewModel>()
                .AddClasses()
                .AsSelf()
                .WithLifetime(lifetime));

        return services;
    }
}