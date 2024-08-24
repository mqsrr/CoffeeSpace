using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Services;

namespace CoffeeSpace.AClient.HttpHandlers;

internal sealed class BearerAuthorizationMessageHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", StaticStorage.JwtToken);
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}