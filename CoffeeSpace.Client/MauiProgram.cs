using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.Extensions;
using CoffeeSpace.Client.Handlers;
using CoffeeSpace.Client.Services.Abstractions;
using CoffeeSpace.Client.Views;
using CoffeeSpace.Client.WebApiClients;
using CommunityToolkit.Maui;
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

        builder.Services.AddPagesFromAssembly<MainView>();        
        builder.Services.AddPagesFromAssembly<MainViewModel>();        

        builder.Services.AddMediator(settings =>
            settings.ServiceLifetime = ServiceLifetime.Scoped);
        
        builder.Services.AddApplicationService<IAuthService>();
        builder.Services.AddApplicationService<AuthHeaderHandler>(ServiceLifetime.Transient);

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