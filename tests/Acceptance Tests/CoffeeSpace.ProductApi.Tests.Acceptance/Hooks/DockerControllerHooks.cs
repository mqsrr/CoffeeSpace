using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using BoDi;
using CoffeeSpace.ProductApi.Tests.Acceptance.Models;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using TechTalk.SpecFlow;

namespace CoffeeSpace.ProductApi.Tests.Acceptance.Hooks;

[Binding]
public sealed class DockerControllerHooks
{
    private readonly IObjectContainer _objectContainer;
    private static ICompositeService _compositeService = null!;
    private static string _token = null!;
    
    public DockerControllerHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeTestRun]
    public static void DockerComposeUp()
    {
        string dockerComposeFilePath = GetDockerComposeLocation("docker-compose-products-api.yml");
         _compositeService = new Builder()
            .UseContainer()
            .FromComposeFile(dockerComposeFilePath)
            .RemoveOrphans()
            .WaitForHttp("coffeespace-productApi-1", "http://localhost:8080/_health", 
                continuation: (response, _) => response.Code == HttpStatusCode.OK ? 0 : 2000)            
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

    [BeforeFeature]
    public static async Task AuthenticateHttpClient()
    {
        var scopedHttpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8081")
        };
        var response = await scopedHttpClient.PostAsJsonAsync("/api/auth/register", new RegisterRequest
        {
            Name = "Testing",
            LastName = "Herll",
            UserName = "UserName",
            Email = "test@gmail.com",
            Password = "Pass1234!"
        });

        _token = await response.Content.ReadAsStringAsync();
    }

    [BeforeScenario]
    private void AddAuthenticatedHttpClient()
    {
        var authenticatedHttpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8080")
        };

        authenticatedHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
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