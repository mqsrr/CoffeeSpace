using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;
using FluentValidation;

namespace CoffeeSpace.OrderingApi.Application.Validators.Buyers;

internal sealed class CreateBuyerRequestValidator : AbstractValidator<CreateBuyerRequest>
{
    public CreateBuyerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}