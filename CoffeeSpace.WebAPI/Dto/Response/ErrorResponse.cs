namespace CoffeeSpace.WebAPI.Dto.Response;

public sealed class ErrorResponse
{
    public ICollection<ErrorModel> Errors { get; set; }
}