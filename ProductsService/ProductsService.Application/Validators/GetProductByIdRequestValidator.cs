using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductById;

namespace ProductsService.Application.Validators;

public class GetProductByIdRequestValidator : AbstractValidator<GetProductByIdRequest>
{
    public GetProductByIdRequestValidator()
    {
        RuleFor(req => req.ProductId)
            .GreaterThan(0)
            .WithMessage("Wrong product id");
    }
}
