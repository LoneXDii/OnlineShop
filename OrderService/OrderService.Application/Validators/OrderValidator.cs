using FluentValidation;
using OrderService.Application.DTO;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Application.Validators;

public class OrderValidator : AbstractValidator<PostOrderDTO>
{
	public OrderValidator()
	{
		RuleFor(order => order.OrderStatus)
			.Must(BeAValidEnumValue<OrderStatuses>)
			.WithMessage("Incorrect order status value");

		RuleFor(order => order.PaymentStatus)
			.Must(BeAValidEnumValue<PaymentStatuses>)
			.WithMessage("Incorrect payment status value");

		RuleFor(order => order.CreatedDate)
			.Must(BeAValidCreatedDate)
			.WithMessage("Incorrect creation date");
	}

	private bool BeAValidEnumValue<T>(int value) where T : Enum
	{
		return Enum.IsDefined(typeof(T), value);
	}

	private bool BeAValidCreatedDate(DateTime createdDate)
	{
		return createdDate < DateTime.Now;
	}
}
