using System.Net;
using BoDi;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using TechTalk.SpecFlow;

namespace CoffeeSpace.IdentityApi.Tests.Acceptance.Hooks;

[Binding]
public sealed class DockerControllerHooks
{
    private readonly IObjectContainer _objectContainer;
    private static ICompositeService _compositeService = null!;
    
    public DockerControllerHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeTestRun]
    public static void DockerComposeUp()
    {
        string dockerComposeFilePath = GetDockerComposeLocation("docker-compose-identity-api.yml");
         _compositeService = new Builder()
            .UseContainer()
            .FromComposeFile(dockerComposeFilePath)
            .RemoveOrphans()
            .WaitForHttp("coffeespace-identityApi-1", "http://localhost:8081/_health", 
                continuation: (response, _) => response.Code == HttpStatusCode.OK ? 0 : 2000)
            .Build()
            .Start();
    }

    [AfterTestRun]
    public static void DockerComposeDown()
    {
        _compositeService.Stop();
        _compositeService.Dispose();
    }

    [BeforeScenario]
    private void AddHttpClient()
    {
        var authenticatedHttpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8081")
        };

        _objectContainer.RegisterInstanceAs(authenticatedHttpClient);
    }
    
    private static string GetDockerComposeLocation(string dockerComposeFileName)
    {
        string directory = Directory.GetCurrentDirectory();
        while (!Directory.EnumerateFiles(directory, "*.yml").Any(file => file.EndsWith(dockerComposeFileName)))
        {
            directory = directory[..directory.LastIndexOf(Path.DirectorySeparatorChar)];
        }

        return Path.Combine(directory, dockerComposeFileName);
    }
}