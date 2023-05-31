using Autofac;
using CoffeeSpace.Client.Handlers;
using CoffeeSpace.Client.Services;
using CoffeeSpace.Client.Services.Abstractions;

namespace CoffeeSpace.Client.Modules;

internal sealed class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AuthService>().As<IAuthService>();
        
        builder.RegisterType<AuthHeaderHandler>().AsSelf();
    }
}