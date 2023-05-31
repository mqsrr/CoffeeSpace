using System.Net.Http.Headers;

namespace CoffeeSpace.Client.Handlers;

internal sealed class AuthHeaderHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var jwtToken = await SecureStorage.GetAsync("jwt-token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}