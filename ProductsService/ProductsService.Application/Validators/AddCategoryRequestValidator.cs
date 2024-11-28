using FluentValidation;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;

namespace ProductsService.Application.Validators;

public class AddCategoryRequestValidator : AbstractValidator<AddCategoryRequest>
{
    public AddCategoryRequestValidator()
    {
        RuleFor(req => req.Name)
            .NotEmpty()
            .WithMessage("Wrong category name");
    }
}
