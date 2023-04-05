using System.Text.Json.Serialization;

namespace CoffeeSpace.Application.Options;

public sealed class JwtSetting
{
    [JsonPropertyName("Issuer")]
    public required string Issuer { get; init; }

    [JsonPropertyName("Audience")]
    public required string Audience { get; init; }

    [JsonPropertyName("Key")]
    public required string Key { get; init; }

    [JsonPropertyName("ExpiredTime")]
    public required int ExpiredTime { get; init; }
}