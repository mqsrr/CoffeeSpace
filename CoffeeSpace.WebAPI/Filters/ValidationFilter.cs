using CoffeeSpace.Contracts.Responses.Errors;
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

        ErrorResponse response = new ErrorResponse
        {
            ErrorModels = new List<ErrorModel>()
        };
        
        foreach (var errors in errorsInModelState)
            response.ErrorModels.Add(new ErrorModel
            {
                FieldName = errors.Key,
                Messages = errors.Messages
            });

        context.Result = new BadRequestObjectResult(response);
    }
}