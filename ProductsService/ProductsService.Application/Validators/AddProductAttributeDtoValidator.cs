using FluentValidation;
using ProductsService.Application.DTO;

namespace ProductsService.Application.Validators;

public class AddProductAttributeDtoValidator : AbstractValidator<AddProductAttributeDTO>
{
    public AddProductAttributeDtoValidator()
    {
        RuleFor(pa => pa.AttributeId)
            .GreaterThan(0)
            .WithMessage("Wrong attribute id");

        RuleFor(pa => pa.ProductId)
            .GreaterThan(0)
            .WithMessage("Wrong product id");

        RuleFor(pa => pa.Value)
            .NotEmpty()
            .WithMessage("Wrong value");
    }
}
