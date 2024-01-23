namespace CoffeeSpace.Domain.Products;

public sealed class ImageInformation
{
    public required string Mime { get; init; }

    public required string Data { get; init; }
}