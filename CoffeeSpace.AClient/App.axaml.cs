using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaWebView;
using CoffeeSpace.AClient.Extensions;
using CoffeeSpace.AClient.HttpHandlers;
using CoffeeSpace.AClient.RefitClients;
using CoffeeSpace.AClient.Services;
using CoffeeSpace.AClient.Services.Abstractions;
using CoffeeSpace.AClient.Settings;
using CoffeeSpace.AClient.Views;
using HotAvalonia;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.AClient;

public class App : Application
{
    public override void Initialize()
    {
        this.EnableHotReload();

        AvaloniaWebViewBuilder.Initialize(default);
        AvaloniaXamlLoader.Load(this);
    }

    public static ServiceProvider Services = null!;
    
    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddMediator();
        
        services.RegisterViews();
        services.RegisterViewModels();

        services.AddOptionsWithValidateOnStart<ApiKeySettings>()
            .Configure(settings =>
            {
                settings.ApiKey = Environment.GetEnvironmentVariable("API_KEY")!;
                settings.HeaderName = Environment.GetEnvironmentVariable("HEADER_NAME")!;
            });
        
        services.AddTransient<BearerAuthorizationMessageHandler>();
        services.AddTransient<ApiKeyAuthorizationMessageHandler>();
        
        services.AddSingleton<IHubConnectionService, HubConnectionService>();
        services.AddWebApiClient<IIdentityWebApi, ApiKeyAuthorizationMessageHandler>()
            .AddWebApiClient<IProductsWebApi>()
            .AddWebApiClient<IOrderingWebApi>()
            .AddWebApiClient<IBuyersWebApi>();
        
        Services = services.BuildServiceProvider(true);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnLastWindowClose;
            desktop.MainWindow = new LoginWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}