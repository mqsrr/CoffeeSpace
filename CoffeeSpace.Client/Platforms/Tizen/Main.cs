namespace CoffeeSpace;

internal class Program : MauiApplication
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    private static void Main(string[] args)
    {
        var app = new Program();
        app.Run(args);
    }
}