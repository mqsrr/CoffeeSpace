using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CoffeeSpace.AClient.Extensions;
using CoffeeSpace.AClient.HttpHandlers;
using CoffeeSpace.AClient.RefitClients;
using CoffeeSpace.AClient.Settings;
using CoffeeSpace.AClient.ViewModels;
using CoffeeSpace.AClient.Views;
using HotAvalonia;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.AClient;

public partial class App : Application
{
    public override void Initialize()
    {
        this.EnableHotReload();
        AvaloniaXamlLoader.Load(this);
    }

    public static ServiceProvider Services = null!;
    
    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddMediator();
        services.RegisterViewModels();

        services.AddTransient<BearerAuthorizationMessageHandler>();
        services.AddTransient<ApiKeyAuthorizationMessageHandler>();
        
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