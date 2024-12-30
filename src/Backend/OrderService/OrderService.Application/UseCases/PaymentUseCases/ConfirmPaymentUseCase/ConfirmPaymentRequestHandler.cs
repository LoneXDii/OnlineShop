using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Application.UseCases.PaymentUseCases.ConfirmPaymentUseCase;

internal class ConfirmPaymentRequestHandler(IPaymentService paymentService, IOrderRepository orderRepository,
    ILogger<ConfirmPaymentRequestHandler> logger)
    : IRequestHandler<ConfirmPaymentRequest>
{
    public async Task Handle(ConfirmPaymentRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to confirm payment for order");

        var orderId = paymentService.GetSuccessPaymentOrderId(request.json, request.signature);

        if(orderId is null)
        {
            logger.LogError($"Wrong order id");

            throw new NotFoundException("No such order");
        }

        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);

        if(order is null)
        {
            logger.LogInformation($"Order with id: {orderId} not found");

            throw new NotFoundException("No such order");
        }

        order.PaymentStatus = PaymentStatuses.Paid;

        await orderRepository.UpdateAsync(order, cancellationToken);

        logger.LogInformation($"Order with id: {orderId} successfully paid");
    }
}
