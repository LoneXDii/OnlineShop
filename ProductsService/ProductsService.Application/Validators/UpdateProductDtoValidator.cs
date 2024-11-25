using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.DTO;

namespace ProductsService.Application.Validators;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDTO>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Name can't be empty");

        RuleFor(p => p.Price)
            .GreaterThan(0)
            .WithMessage("Wrong price");

        RuleFor(p => p.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Wrong quantity");

        RuleFor(p => p.CategoryId)
            .GreaterThan(0)
            .WithMessage("Wrong category");

        RuleFor(p => p.Image)
            .Must(BeAnImage)
            .WithMessage("You should upload an image")
            .When(p => p.Image != null);
    }

    private bool BeAnImage(IFormFile? file)
    {
        return file != null && file.ContentType.StartsWith("image/");
    }
}
