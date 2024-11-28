using FluentValidation;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetUniqueCategoryAttributesValues;

namespace ProductsService.Application.Validators.Categories;

public class GetUniqueCategoryAttributesValuesRequestValidator : AbstractValidator<GetUniqueCategoryAttributesValuesRequest>
{
    public GetUniqueCategoryAttributesValuesRequestValidator()
    {
        RuleFor(req => req.categoryId)
            .GreaterThan(0)
            .WithMessage("Wrong category id");
    }
}
