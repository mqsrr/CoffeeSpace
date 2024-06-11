using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using FluentValidation;
using SixLabors.ImageSharp;

namespace CoffeeSpace.ProductApi.Application.Validators;

internal sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {

        RuleFor(product => product.Title)
            .NotEmpty()
            .NotNull()
            .MaximumLength(64);

        RuleFor(product => product.Image)
            .NotNull()
            .Must(image =>
            {
                try
                {
                    Image.Identify(image.OpenReadStream());
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            })
            .WithMessage(ValidationErrorMessages.InvalidImageType);

        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.UnitPrice)
            .NotNull()
            .GreaterThan(0)
            .LessThan(99f);

        RuleFor(x => x.Quantity)
            .NotNull()
            .GreaterThan(0);
    }
}