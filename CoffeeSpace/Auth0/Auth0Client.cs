using IdentityModel.OidcClient;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace CoffeeSpace.Auth0;

public class Auth0Client
{
    private readonly OidcClient _oidcClient;

    public IBrowser Browser
    {
        get => _oidcClient.Options.Browser;
        set => _oidcClient.Options.Browser = value;
    }

    public Auth0Client(Auth0ClientOptions options) =>
        _oidcClient = new OidcClient(new OidcClientOptions
        {
            Authority = $"https://{options.Domain}",
            ClientId = options.ClientId,
            Scope = options.Scope,
            RedirectUri = options.RedirectUri,
            Browser = options.Browser
        });

    public Task<LoginResult> LoginAsync(CancellationToken token = default) =>
        _oidcClient.LoginAsync(cancellationToken: token);
}