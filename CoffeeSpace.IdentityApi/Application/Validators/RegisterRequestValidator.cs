using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;
using FluentValidation;

namespace CoffeeSpace.IdentityApi.Application.Validators;

internal sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.LastName)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.UserName)
            .NotEmpty()
            .NotNull();
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull()
            .Matches(@"^(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z])(?=\D*\d)(?=[^!#%]*[!#%])[A-Za-z0-9!#%]{6,32}$");        
    }
}