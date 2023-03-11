using CoffeeSpace.Contracts.Requests.Customer;
using FluentValidation;

namespace CoffeeSpace.WebAPI.Validators;

public sealed class CustomerRegisterModelValidator : AbstractValidator<RegisterRequest>
{
    public CustomerRegisterModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
        
        RuleFor(x => x.LastName)
            .NotEmpty();
        
        RuleFor(x => x.Username)
            .NotEmpty();
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(32)
            .Matches(@"^(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z])(?=\D*\d)(?=[^!#%]*[!#%])[A-Za-z0-9!#%]{6,32}$");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Address)
            .NotEmpty()
            .Must(x => !string.IsNullOrEmpty(x.Id));
        
        RuleFor(x => x.PaymentInfo)
            .NotEmpty()
            .Must(x => !string.IsNullOrEmpty(x.Id));

    }
}