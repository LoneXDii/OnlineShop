using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductById;

namespace ProductsService.Application.Validators.Products;

public class GetProductByIdRequestValidator : AbstractValidator<GetProductByIdRequest>
{
    public GetProductByIdRequestValidator()
    {
        RuleFor(req => req.ProductId)
            .GreaterThan(0)
            .WithMessage("Wrong product id");
    }
}
