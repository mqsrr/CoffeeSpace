using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.Extensions;
using CoffeeSpace.Client.Handlers;
using CoffeeSpace.Client.Services.Abstractions;
using CoffeeSpace.Client.Views;
using CoffeeSpace.Client.WebApiClients;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace CoffeeSpace.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureImageSources();

        builder.Services.AddPagesFromAssembly<MainView>(ServiceLifetime.Scoped);        
        builder.Services.AddPagesFromAssembly<MainViewModel>(ServiceLifetime.Scoped);        

        builder.Services.AddMediator();
        builder.Services.AddApplicationService<IAuthService>();

        builder.Services.AddTransient<AuthHeaderHandler>();

        builder.Services
            .AddWebApiClient<IIdentityWebApi>(requiresAuthorization: false)
            .AddWebApiClient<IBuyersWebApi>(requiresAuthorization: true)
            .AddWebApiClient<IProductsWebApi>(requiresAuthorization: true)
            .AddWebApiClient<IOrderingWebApi>(requiresAuthorization: true);
        
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}