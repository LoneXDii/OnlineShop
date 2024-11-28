using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;

namespace ProductsService.Application.Validators;

public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
{
    public AddProductRequestValidator()
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
            .Must(BeAnImage)
            .WithMessage("You should upload only image files")
            .When(req => req.Image is not null);

        RuleFor(req => req.Attributes)
            .Must(BeAnIdsArray)
            .WithMessage("Wrong param id");
    }

    private bool BeAnImage(IFormFile? file)
    {
        return file != null && file.ContentType.StartsWith("image/");
    }

    private bool BeAnIdsArray(int[] ids)
    {
        foreach (var id in ids)
        {
            if(id <= 0)
            {
                return false;
            }
        }

        return true;
    }
}
