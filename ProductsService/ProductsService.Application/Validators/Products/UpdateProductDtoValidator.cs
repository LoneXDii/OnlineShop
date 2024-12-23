using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.DTO;

namespace ProductsService.Application.Validators.Products;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDTO>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(req => req.Name)
            .NotEmpty()
            .WithMessage("Wrong product name");

        RuleFor(req => req.Price)
            .GreaterThan(0)
            .WithMessage("Wrong price");

        RuleFor(req => req.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Wrong quantity");

        RuleFor(req => req.Image)
            .Must(IsAnImage)
            .WithMessage("You should upload only image files")
            .When(req => req.Image is not null);
    }

    private bool IsAnImage(IFormFile? file)
    {
        return file != null && file.ContentType.StartsWith("image/");
    }
}
