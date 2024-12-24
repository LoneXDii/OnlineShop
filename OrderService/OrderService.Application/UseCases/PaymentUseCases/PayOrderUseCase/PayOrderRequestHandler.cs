using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Application.UseCases.PaymentUseCases.PayOrderUseCase;

internal class PayOrderRequestHandler(IOrderRepository orderRepository, IPaymentService paymentService,
    ILogger<PayOrderRequestHandler> logger)
    : IRequestHandler<PayOrderRequest, string>
{
    public async Task<string> Handle(PayOrderRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User with id: {request.userId} trying to pay order with id: {request.orderId}");

        var order = await orderRepository.GetByIdAsync(request.orderId, cancellationToken);

        if(order is null)
        {
            logger.LogError($"Order with id: {request.orderId} not found");

            throw new NotFoundException("No such order");
        }

        if(order.UserId != request.userId)
        {
            logger.LogError($"User with id: {request.userId} has no access to order with id: {request.orderId}");

            throw new AccessDeniedException("You have no access to this order");
        }

        if(order.PaymentStatus == PaymentStatuses.Paid)
        {
            logger.LogInformation($"Order with id: {order.Id} already paid");

            throw new BadRequestException("This order is already paid");
        }

        return await paymentService.PayAsync(order, request.stribeId);
    }
}
