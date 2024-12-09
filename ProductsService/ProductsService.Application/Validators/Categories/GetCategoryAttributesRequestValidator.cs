using FluentValidation;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryAttributes;

namespace ProductsService.Application.Validators.Categories;

public class GetCategoryAttributesRequestValidator : AbstractValidator<GetCategoryAttributesRequest>
{
    public GetCategoryAttributesRequestValidator()
    {
        RuleFor(req => req.categoryId)
            .GreaterThan(0)
            .WithMessage("Wrong category id");
    }
}
