namespace CoffeeSpace.PaymentService.Application.Settings;

internal sealed class PaypalAuthenticationSettings
{
    public const string SectionName = "Paypal"; 
    
    public required string ClientId { get; init; }

    public required string ClientSecret { get; init; }
}