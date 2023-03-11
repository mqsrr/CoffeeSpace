using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace CoffeeSpace.Auth0;

public class Auth0ClientOptions
{
    public string Scope { get; set; }
    public string Domain { get; set; }
    public string ClientId { get; set; }
    public string RedirectUri { get; set; }

    public IBrowser Browser { get; set; }
    
    public Auth0ClientOptions()
    {
        Scope = "openid";
        RedirectUri = "myapp://callback";
        Browser = new WebBrowserAuthenticator();
    }
}