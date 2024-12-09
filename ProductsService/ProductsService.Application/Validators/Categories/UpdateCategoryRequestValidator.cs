using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;

namespace ProductsService.Application.Validators.Categories;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(req => req.Name)
            .NotEmpty()
            .WithMessage("Wrong category name");

        RuleFor(req => req.CategoryId)
            .GreaterThan(0)
            .WithMessage("Wrong category id");

        RuleFor(req => req.Image)
            .Must(BeAnImage)
            .WithMessage("You should upload only image files")
            .When(req => req.Image is not null);
    }

    private bool BeAnImage(IFormFile? file)
    {
        return file != null && file.ContentType.StartsWith("image/");
    }
}
