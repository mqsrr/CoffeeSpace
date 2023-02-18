using System.Collections;

namespace CoffeeSpace.WebAPI.Dto.Response;

public sealed class ErrorModel
{
    public string FieldName { get; set; } = default!;
    public IEnumerable<string> Messages { get; set; } = default!;
}