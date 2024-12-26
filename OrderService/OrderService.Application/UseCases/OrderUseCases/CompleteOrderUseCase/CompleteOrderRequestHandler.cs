using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Application.UseCases.OrderUseCases.CompleteOrderUseCase;

internal class CompleteOrderRequestHandler(IOrderRepository orderRepository, IProducerService producerService,
    ILogger<CompleteOrderRequestHandler> logger)
    : IRequestHandler<CompleteOrderRequest>
{
    public async Task Handle(CompleteOrderRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to compelete order with id: {request.orderId}");

        var order = await orderRepository.GetByIdAsync(request.orderId, cancellationToken);

        if (order is null)
        {
            logger.LogError($"Order with id: {request.orderId} not found");

            throw new NotFoundException("No such order");
        }

        if (order.OrderStatus != OrderStatuses.Confirmed)
        {
            logger.LogError($"Order status must be {OrderStatuses.Confirmed}, current status is {order.OrderStatus}");

            throw new BadRequestException("Can complete only confirmed orders");
        }

        if (order.PaymentStatus == PaymentStatuses.NotPaid)
        {
            logger.LogError($"Order payment status must be {PaymentStatuses.NotPaid}, current status is {order.PaymentStatus}");

            throw new BadRequestException("Can't complete unpaid order'");
        }

        order.OrderStatus = OrderStatuses.Completed;

        await orderRepository.UpdateAsync(order, cancellationToken);

        await producerService.ProduceOrderActionsAsync(order, cancellationToken);

        logger.LogInformation($"Order with id: {request.orderId} successfully completed");
    }
}
