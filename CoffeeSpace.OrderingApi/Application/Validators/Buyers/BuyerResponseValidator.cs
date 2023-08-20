using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Buyers;
using FluentValidation;

namespace CoffeeSpace.OrderingApi.Application.Validators.Buyers;

internal sealed class BuyerResponseValidator : AbstractValidator<BuyerResponse>
{
    public BuyerResponseValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
            
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();
    }
}