using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Application.UseCases.PaymentUseCases.PayOrderUseCase;

internal class PayOrderRequestHandler(IOrderRepository dbService, IPaymentService paymentService)
    : IRequestHandler<PayOrderRequest, string>
{
    public async Task<string> Handle(PayOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await dbService.GetByIdAsync(request.orderId, cancellationToken);

        if(order is null)
        {
            throw new NotFoundException("No such order");
        }

        if(order.UserId != request.userId)
        {
            throw new AccessDeniedException("You have no access to this order");
        }

        if(order.PaymentStatus == PaymentStatuses.Paid)
        {
            throw new BadRequestException("This order is already paid");
        }

        var stribeUrl = await paymentService.PayAsync(order, request.stribeId);

        return stribeUrl;
    }
}
