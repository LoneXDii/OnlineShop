using FluentValidation;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

namespace ProductsService.Application.Validators;

public class UpdateProductAttributeRequestValidator : AbstractValidator<UpdateProductAttributeRequest>
{
    public UpdateProductAttributeRequestValidator()
    {
        RuleFor(pa => pa.ProductAttributeId)
            .GreaterThan(1)
            .WithMessage("Wrong product attribute id");

        RuleFor(pa => pa.Value)
            .NotEmpty()
            .WithMessage("Wrong value");
    }
}
