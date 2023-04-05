using System.Reflection;
using CoffeeSpace.Extensions;
using CoffeeSpace.Initializers;
using CoffeeSpace.Services;
using CommunityToolkit.Maui;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CustomerService = CoffeeSpace.Services.CustomerService;

namespace CoffeeSpace;

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
			.RegisterViews()
			.RegisterViewModels()
			.ConfigureImageSources();

		builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

		builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

		builder.Services.AddScoped<IAuthService, AuthService>();
		builder.Services.AddScoped<ICustomerService, CustomerService>();
		builder.Services.AddScoped<IOrderItemService, OrderItemService>();

		builder.Services.AddAuth0Client();
		builder.Services.AddOidClient();
		builder.Services.AddSignalRHubConnection();

		builder.Services.AddTransient<HttpClient>();
		builder.Services.AddTransient<IMauiInitializeService, MainViewInitializer>();
		
#if DEBUG
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}
