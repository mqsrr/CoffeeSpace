using CoffeeSpace.WebAPI.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoffeeSpace.WebAPI.Filters;

public sealed class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid) 
            await next();

        var errorsInModelState = context.ModelState
            .Where(x => x.Value!.Errors.Count > 0)
            .Select(x => new
            {
                Key = x.Key,
                Messages = x.Value!.Errors.Select(e => e.ErrorMessage)
            });

        ErrorResponse response = new ErrorResponse();
        
        foreach (var errors in errorsInModelState)
            response.Errors.Add(new ErrorModel
            {
                FieldName = errors.Key,
                Messages = errors.Messages
            });

        context.Result = new BadRequestObjectResult(response);
    }
}