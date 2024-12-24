using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Application.UseCases.OrderUseCases.ConfirmOrderUseCase;

internal class ConfirmOrderRequestHandler(IOrderRepository orderRepository, IProducerService producerService,
    ILogger<ConfirmOrderRequestHandler> logger)
    : IRequestHandler<ConfirmOrderRequest>
{
    public async Task Handle(ConfirmOrderRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to confirm order with id: {request.orderId}");

        var order = await orderRepository.GetByIdAsync(request.orderId, cancellationToken);

        if (order is null)
        {
            logger.LogError($"Order with id: {request.orderId} not found");

            throw new NotFoundException("No such order");
        }

        if (order.OrderStatus != OrderStatuses.Created)
        {
            logger.LogError($"Order status must be {OrderStatuses.Created}, current status is {order.OrderStatus}");

            throw new BadRequestException("Order confirmation error, invalid order status");
        }

        order.OrderStatus = OrderStatuses.Confirmed;

        await orderRepository.UpdateAsync(order, cancellationToken);

        await producerService.ProduceOrderActionsAsync(order, cancellationToken);

        logger.LogInformation($"Order with id: {request.orderId} successfully confirmed");
    }
}
