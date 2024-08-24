using System.Runtime.InteropServices;
using Azure.Identity;
using CoffeeSpace.Shared.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CoffeeSpace.Shared.Extensions;

public static class AzureKeyVaultExtensions
{
    public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder configuration)
    {
        if (!Environment.GetEnvironmentVariable(Environments.Staging).IsNullOrEmpty())
        {
            return configuration;
        }
        
        return configuration.AddEnvironmentVariables()
            .AddAzureKeyVault(new Uri($"https://{Environment.GetEnvironmentVariable("AZURE_VAULT_NAME")}.vault.azure.net"),
                new DefaultAzureCredential(),
                new PrefixKeyVaultSecretManager("CoffeeSpace"));
    }
}