using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using FluentValidation;
using SixLabors.ImageSharp;

namespace CoffeeSpace.ProductApi.Application.Validators;

internal sealed class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull();

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

        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.UnitPrice)
            .NotNull()
            .GreaterThan(0)
            .LessThan(99);

        RuleFor(x => x.Quantity)
            .NotNull()
            .GreaterThan(0);
    }
}