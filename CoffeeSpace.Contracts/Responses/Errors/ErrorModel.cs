
namespace CoffeeSpace.Contracts.Responses.Errors;

public sealed class ErrorModel
{
    public required string FieldName { get; init; }
    public required IEnumerable<string> Messages { get; set; }
}
