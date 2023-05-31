using Autofac;
using CoffeeSpace.Client.Views;

namespace CoffeeSpace.Client.Modules;

internal sealed class ViewsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(MainView).Assembly);
    }
}