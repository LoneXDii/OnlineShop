using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

namespace ProductsService.Application.Validators;

public class UpdateProductAttributeRequestValidator : AbstractValidator<UpdateProductAttributeRequest>
{
    public UpdateProductAttributeRequestValidator()
    {
        RuleFor(pa => pa.ProductId)
            .GreaterThan(0)
            .WithMessage("Wrong product id");

        RuleFor(pa => pa.NewValueId)
            .GreaterThan(0)
            .WithMessage("Wrong attribute id");

        RuleFor(pa => pa.OldValueId)
            .GreaterThan(0)
            .WithMessage("Wrong attribute id");
    }
}
