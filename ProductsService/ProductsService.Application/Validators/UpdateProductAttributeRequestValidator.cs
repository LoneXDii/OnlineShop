using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

namespace ProductsService.Application.Validators;

public class UpdateProductAttributeRequestValidator : AbstractValidator<UpdateProductAttributeRequest>
{
    public UpdateProductAttributeRequestValidator()
    {
        RuleFor(req => req.ProductId)
            .GreaterThan(0)
            .WithMessage("Wrong product id");

        RuleFor(req => req.NewAttributeId)
            .GreaterThan(0)
            .WithMessage("Wrong attribute id");

        RuleFor(req => req.OldAttributeId)
            .GreaterThan(0)
            .WithMessage("Wrong attribute id");
    }
}
