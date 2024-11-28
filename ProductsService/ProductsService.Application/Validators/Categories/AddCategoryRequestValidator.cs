using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;

namespace ProductsService.Application.Validators.Categories;

public class AddCategoryRequestValidator : AbstractValidator<AddCategoryRequest>
{
    public AddCategoryRequestValidator()
    {
        RuleFor(req => req.Name)
            .NotEmpty()
            .WithMessage("Wrong category name");

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
