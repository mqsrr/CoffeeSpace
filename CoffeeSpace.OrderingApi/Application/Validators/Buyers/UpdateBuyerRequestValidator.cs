using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;
using FluentValidation;

namespace CoffeeSpace.OrderingApi.Application.Validators.Buyers;

internal sealed class UpdateBuyerRequestValidator : AbstractValidator<UpdateBuyerRequest>
{
    public UpdateBuyerRequestValidator()
    {
        RuleFor(x => x.Id)
            .Empty();

        RuleFor(x => x.Name)
            .NotEmpty();
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}