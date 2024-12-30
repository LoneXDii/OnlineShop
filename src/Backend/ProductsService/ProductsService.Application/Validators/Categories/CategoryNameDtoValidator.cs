using FluentValidation;
using ProductsService.Application.DTO;

namespace ProductsService.Application.Validators.Categories;

public class CategoryNameDtoValidator : AbstractValidator<CategoryNameDTO>
{
    public CategoryNameDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithMessage("Wrong name");
    }
}
