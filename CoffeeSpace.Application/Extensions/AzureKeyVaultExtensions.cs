using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using CoffeeSpace.Application.Services;
using Microsoft.Extensions.Configuration;

namespace CoffeeSpace.Application.Extensions;

public static class AzureKeyVaultExtensions
{
    public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder configuration)
    {
        return configuration.AddEnvironmentVariables()
            .AddAzureKeyVault(new Uri($"https://{Environment.GetEnvironmentVariable("AZURE_VAULT_NAME")}.vault.azure.net/"),
                new EnvironmentCredential(),
                new PrefixKeyVaultSecretManager("CoffeeSpace"));
    }
}