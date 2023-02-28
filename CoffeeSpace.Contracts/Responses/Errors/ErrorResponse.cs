namespace CoffeeSpace.Contracts.Responses.Errors;

public sealed class ErrorResponse
{
    public required ICollection<ErrorModel> ErrorModels { get; init; }
}