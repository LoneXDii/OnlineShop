using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteProduct;

namespace ProductsService.Application.Validators.Products;

public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductRequestValidator()
    {
        RuleFor(req => req.ProductId)
            .GreaterThan(0)
            .WithMessage("Wrong product id");
    }
}
