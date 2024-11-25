using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteProduct;

namespace ProductsService.Application.Validators;

public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductRequestValidator()
    {
        RuleFor(req => req.productId)
            .GreaterThan(0)
            .WithMessage("Wrong product id");
    }
}
