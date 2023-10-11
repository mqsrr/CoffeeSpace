using CoffeeSpace.IdentityApi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace CoffeeSpace.IdentityApi.Filters;

internal sealed class ApiKeyAuthorizationFilter : IAuthorizationFilter
{
    private readonly ApiKeySettings _apiKeySettings;

    public ApiKeyAuthorizationFilter(IOptions<ApiKeySettings> apiKeySettings)
    {
        _apiKeySettings = apiKeySettings.Value;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(_apiKeySettings.HeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key Is Missing");
            return;
        }

        string confirmedApiKey = _apiKeySettings.ApiKey;
        if (extractedApiKey != confirmedApiKey)
        {
            context.Result = new UnauthorizedObjectResult("Incorrect API Key Value");
        }
    }
}