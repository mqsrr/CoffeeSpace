using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;
using FluentValidation;

namespace CoffeeSpace.IdentityApi.Application.Validators;

internal sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull();

        RuleFor(x => x.LastName)
            .NotNull();

        RuleFor(x => x.UserName)
            .NotNull();
        
        RuleFor(x => x.Email)
            .NotNull()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotNull()
            .Matches(@"^(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z])(?=\D*\d)(?=[^!#%]*[!#%])[A-Za-z0-9!#%]{6,32}$");        
    }
}