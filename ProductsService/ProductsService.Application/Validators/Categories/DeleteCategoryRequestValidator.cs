using FluentValidation;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.DeleteCategory;

namespace ProductsService.Application.Validators.Categories;

public class DeleteCategoryRequestValidator : AbstractValidator<DeleteCategoryRequest>
{
    public DeleteCategoryRequestValidator()
    {
        RuleFor(req => req.categoryId)
            .GreaterThan(0)
            .WithMessage("Wrong category id");
    }
}
