using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;

namespace ProductsService.Application.Validators;

public class DeleteAttributeFromProductRequestValidator : AbstractValidator<DeleteAttributeFromProductRequest>
{
    public DeleteAttributeFromProductRequestValidator()
    {
        RuleFor(req => req.productAttributeId)
            .GreaterThan(0)
            .WithMessage("Wrong attribute id");
    }
}
