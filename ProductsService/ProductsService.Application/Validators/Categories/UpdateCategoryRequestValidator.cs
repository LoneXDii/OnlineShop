using FluentValidation;
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
    }
}
