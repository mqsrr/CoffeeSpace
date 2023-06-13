using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.Extensions;
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
        
        builder.Services
            .AddWebApiClient<IIdentityWebApi>()
            .AddAuthenticationWebApiClient<IBuyersWebApi>()
            .AddAuthenticationWebApiClient<IProductsWebApi>();

        builder.Services.AddApplicationService<IAuthService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}