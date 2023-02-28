using CoffeeSpace.Contracts.Requests.Customer;
using FluentValidation;

namespace CoffeeSpace.WebAPI.Validators;

public sealed class CustomerLoginModelValidator : AbstractValidator<LoginRequest>
{
    public CustomerLoginModelValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(32)
            .Matches(@"^(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z])(?=\D*\d)(?=[^!#%]*[!#%])[A-Za-z0-9!#%]{6,32}$");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}