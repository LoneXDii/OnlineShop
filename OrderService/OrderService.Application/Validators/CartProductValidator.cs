using FluentValidation;
using OrderService.Application.DTO;

namespace OrderService.Application.Validators;

public class CartProductValidator : AbstractValidator<CartProductDTO>
{
    public CartProductValidator()
    {
        RuleFor(product => product.Quantity)
            .GreaterThan(0)
            .WithMessage("Incorrect quantity");
    }
}
