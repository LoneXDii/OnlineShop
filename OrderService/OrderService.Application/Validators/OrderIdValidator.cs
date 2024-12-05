using FluentValidation;
using OrderService.Application.DTO;

namespace OrderService.Application.Validators;

public class OrderIdValidator : AbstractValidator<OrderIdDTO>
{
    public OrderIdValidator()
    {
        RuleFor(orderId => orderId.OrderId)
            .Matches(@"^[a-fA-F0-9]{24}$")
            .WithMessage("Wrong order id format");
    }
}
