using Newtonsoft.Json;

namespace CoffeeSpace.PaymentService.Models;

public record OrderApprovedWebhookEvent(
    [JsonProperty("id")] string Id,
    [JsonProperty("create_time")] DateTime CreateTime,
    [JsonProperty("resource_type")] string ResourceType,
    [JsonProperty("event_type")] string EventType,
    [JsonProperty("summary")] string Summary,
    [JsonProperty("resource")] Resource Resource,
    [JsonProperty("links")] List<Link> Links,
    [JsonProperty("event_version")] string EventVersion,
    [JsonProperty("resource_version")] string ResourceVersion
);

public record Link(
    [JsonProperty("href")] string Href,
    [JsonProperty("rel")] string Rel,
    [JsonProperty("method")] string Method,
    [JsonProperty("encType")] string EncType
);

public record Resource(
    [JsonProperty("update_time")] DateTime UpdateTime,
    [JsonProperty("create_time")] DateTime CreateTime,
    [JsonProperty("purchase_units")] List<PurchaseUnit> PurchaseUnits,
    [JsonProperty("links")] List<Link> Links,
    [JsonProperty("id")] string Id,
    [JsonProperty("intent")] string Intent,
    [JsonProperty("payer")] Payer Payer,
    [JsonProperty("status")] string Status
);

public record PurchaseUnit(
    [JsonProperty("reference_id")] string ReferenceId,
    [JsonProperty("amount")] Amount Amount,
    [JsonProperty("payee")] Payee Payee,
    [JsonProperty("shipping")] Shipping Shipping
);

public record Amount(
    [JsonProperty("currency_code")] string CurrencyCode,
    [JsonProperty("value")] string Value
);

public record Address(
    [JsonProperty("address_line_1")] string AddressLine1,
    [JsonProperty("address_line_2")] string AddressLine2,
    [JsonProperty("admin_area_2")] string AdminArea2,
    [JsonProperty("admin_area_1")] string AdminArea1,
    [JsonProperty("postal_code")] string PostalCode,
    [JsonProperty("country_code")] string CountryCode
);

public record Name(
    [JsonProperty("given_name")] string GivenName,
    [JsonProperty("surname")] string Surname
);

public record Payee(
    [JsonProperty("email_address")] string EmailAddress
);

public record Payer(
    [JsonProperty("name")] Name Name,
    [JsonProperty("email_address")] string EmailAddress,
    [JsonProperty("payer_id")] string PayerId
);

public record Shipping(
    [JsonProperty("method")] string Method,
    [JsonProperty("address")] Address Address
);