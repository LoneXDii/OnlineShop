using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddAttributeToProduct;

namespace ProductsService.Application.Validators;

public class AddAttributeToProductRequestValidator : AbstractValidator<AddAttributeToProductRequest>
{
    public AddAttributeToProductRequestValidator()
    {
        RuleFor(req => req.ProductId)
            .GreaterThan(0)
            .WithMessage("Wrong product id");

        RuleFor(req => req.AttributeId)
            .GreaterThan(0)
            .WithMessage("Wrong attribute id");
    }
}
