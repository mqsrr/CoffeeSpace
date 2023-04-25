using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Buyers;
using FluentValidation;

namespace CoffeeSpace.OrderingApi.Application.Validators.Buyers;

internal sealed class BuyerResponseValidators : AbstractValidator<BuyerResponse>
{
    public BuyerResponseValidators()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        
        RuleFor(x => x.Name)
            .NotEmpty();
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}