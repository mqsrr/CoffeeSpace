using CoffeeSpace.Views;

namespace CoffeeSpace.Extensions;

public static class RegisterViewsExtension
{
    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainView>();
        builder.Services.AddSingleton<LoginView>();
        builder.Services.AddSingleton<OrderView>();
        builder.Services.AddSingleton<CartView>();

        return builder;
    }
}
