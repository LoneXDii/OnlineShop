using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;

namespace OrderService.Application.UseCases.PaymentUseCases.PayOrderUseCase;

internal class PayOrderRequestHandler(IDbService dbService, IPaymentService paymentService)
	: IRequestHandler<PayOrderRequest, string>
{
	public async Task<string> Handle(PayOrderRequest request, CancellationToken cancellationToken)
	{
		var order = await dbService.GetOrderByIdAsync(request.orderId);

		if(order is null)
		{
			throw new NotFoundException("No such order");
		}

		if(order.UserId != request.userId)
		{
			throw new AccessDeniedException("You have no access to this order");
		}

		var stribeUrl = await paymentService.PayAsync(order, request.stribeId);
		return stribeUrl;
	}
}
