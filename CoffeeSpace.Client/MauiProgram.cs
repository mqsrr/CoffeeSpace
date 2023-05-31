using Autofac;
using Autofac.Extensions.DependencyInjection;
using CoffeeSpace.Client.Extensions;
using CoffeeSpace.Client.Modules;
using CoffeeSpace.Client.WebApiClients;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Refit;

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
            .ConfigureImageSources()
            .ConfigureContainer(new AutofacServiceProviderFactory(), containerBuilder =>
                containerBuilder.RegisterAssemblyModules(typeof(IModulesFlag).Assembly));

        builder.Services.AddMediator(settings =>
            settings.ServiceLifetime = ServiceLifetime.Scoped);
        
        builder.Services
            .AddWebApiClient<IIdentityWebApi>()
            .AddAuthenticationWebApiClient<IBuyersWebApi>()
            .AddAuthenticationWebApiClient<IProductsWebApi>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}