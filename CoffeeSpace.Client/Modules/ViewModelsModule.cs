using Autofac;
using CoffeeSpace.Client._ViewModels;

namespace CoffeeSpace.Client.Modules;

internal sealed class ViewModelsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(MainViewModel).Assembly).AsSelf();
    }
}