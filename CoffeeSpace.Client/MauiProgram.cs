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

        builder.Services.AddApplicationService<IAuthService>();
        builder.Services.AddApplicationService<IHubConnectionService>(ServiceLifetime.Singleton);

        builder.Services.AddApiKeyOptions(builder.Configuration);
        builder.Services.AddMediator();
        
        builder.Services.AddTransient<BearerAuthorizationMessageHandler>();
        builder.Services.AddTransient<ApiKeyAuthorizationMessageHandler>();

        builder.Services
            .AddWebApiClient<IIdentityWebApi, ApiKeyAuthorizationMessageHandler>()
            .AddWebApiClient<IBuyersWebApi>()
            .AddWebApiClient<IProductsWebApi>()
            .AddWebApiClient<IOrderingWebApi>();
        
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}