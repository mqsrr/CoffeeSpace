using System.Text.Json.Serialization;

namespace CoffeeSpace.WebAPI.Options;

public sealed class JwtOption
{
    [JsonPropertyName("Issuer")]
    public string Issuer { get; set; } = default!;

    [JsonPropertyName("Audience")]
    public string Audience { get; set; } = default!;

    [JsonPropertyName("Key")]
    public string Key { get; set; } = default!;

    [JsonPropertyName("ExpiredTime")]
    public int ExpiredTime { get; set; } = default!;
}