using FluentValidation;
using OrderService.Application.UseCases.CartUseCases.RemoveItemFromCartUseCase;

namespace OrderService.Application.Validators;

public class RemoveItemFromCartRequestValidator : AbstractValidator<RemoveItemFromCartRequest>
{
    public RemoveItemFromCartRequestValidator()
    {
        RuleFor(req => req.itemId)
            .GreaterThan(0)
            .WithMessage("Wrong item id");
    }
}
