using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Application.UseCases.PaymentUseCases.ConfirmPaymentUseCase;

internal class ConfirmPaymentRequestHandler(IPaymentService paymentService, IOrderRepository orderRepository)
    : IRequestHandler<ConfirmPaymentRequest>
{
    public async Task Handle(ConfirmPaymentRequest request, CancellationToken cancellationToken)
    {
        var orderId = paymentService.GetSuccessPaymentOrderId(request.json, request.signature);

        if(orderId is null)
        {
            throw new NotFoundException("No such order");
        }

        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);

        if(order is null)
        {
            throw new NotFoundException("No such order");
        }

        order.PaymentStatus = PaymentStatuses.Paid;

        await orderRepository.UpdateAsync(order, cancellationToken);
    }
}
