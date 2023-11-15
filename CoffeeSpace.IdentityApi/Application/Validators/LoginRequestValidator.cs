using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Login;
using FluentValidation;

namespace CoffeeSpace.IdentityApi.Application.Validators;

internal sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .Matches(@"^(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z])(?=\D*\d)(?=[^!#%]*[!#%])[A-Za-z0-9!#%]{6,32}");
    }
}