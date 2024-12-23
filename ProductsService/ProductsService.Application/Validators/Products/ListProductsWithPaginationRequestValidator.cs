using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;

namespace ProductsService.Application.Validators.Products;

public class ListProductsWithPaginationRequestValidator : AbstractValidator<ListProductsWithPaginationRequest>
{
    public ListProductsWithPaginationRequestValidator()
    {
        RuleFor(req => req.MinPrice)
            .GreaterThan(0)
            .WithMessage("Wrong min price")
            .When(req => req.MinPrice is not null);

        RuleFor(req => req.MaxPrice)
            .GreaterThan(0)
            .WithMessage("Wrong max price")
            .When(req => req.MaxPrice is not null);

        RuleFor(req => req.MaxPrice)
            .GreaterThanOrEqualTo(req => req.MinPrice)
            .WithMessage("Max price cant be less than min price")
            .When(req => req.MaxPrice is not null && req.MinPrice is not null);

        RuleFor(req => req.CategoryId)
            .GreaterThan(0)
            .WithMessage("Wrong category id")
            .When(req => req.CategoryId is not null);

        RuleFor(req => req.ValuesIds)
            .Must(valuesIds => valuesIds.All(id => id > 0))
            .WithMessage("Wrong values ids")
            .When(req => req.ValuesIds is not null);
    }
}
