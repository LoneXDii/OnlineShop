using FluentValidation;
using OrderService.Application.DTO;

namespace OrderService.Application.Validators;

public class QuantityDtoValidator : AbstractValidator<QuantityDTO>
{
    public QuantityDtoValidator()
    {
        RuleFor(quantity => quantity.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Wrong quantity");
    }
}
