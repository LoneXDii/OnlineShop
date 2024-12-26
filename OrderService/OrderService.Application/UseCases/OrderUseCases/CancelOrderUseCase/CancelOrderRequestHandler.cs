using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Application.UseCases.OrderUseCases.CancelOrderUseCase;

internal class CancelOrderRequestHandler(IOrderRepository orderRepository, IProductService productService, 
    IProducerService producerService, ILogger<CancelOrderRequestHandler> logger)
    : IRequestHandler<CancelOrderRequest>
{
    public async Task Handle(CancelOrderRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to cancel order with id: {request.orderId}");

        var order = await orderRepository.GetByIdAsync(request.orderId, cancellationToken);

        if (order is null)
        {
            logger.LogError($"Order with id: {request.orderId} not found");

            throw new NotFoundException("No such order");
        }

        if (request.userId is not null)
        {
            if (order.UserId != request.userId)
            {
                logger.LogError($"user with id: {request.userId} has no access to order with id: {request.orderId}");

                throw new AccessDeniedException("You dont have access to this order");
            }
        }

        if(order.OrderStatus == OrderStatuses.Completed)
        {
            logger.LogError($"Order status can't be {OrderStatuses.Completed}");

            throw new BadRequestException("Cannot cancel completed order");
        }

        if (order.OrderStatus == OrderStatuses.Cancelled)
        {
            logger.LogError($"Order status can't be {OrderStatuses.Cancelled}");

            throw new BadRequestException("This order is already cancelled");
        }

        order.OrderStatus = OrderStatuses.Cancelled;

        await orderRepository.UpdateAsync(order, cancellationToken);

        await productService.ReturnProductsAsync(order.Products, cancellationToken);

        await producerService.ProduceOrderActionsAsync(order, cancellationToken);

        logger.LogInformation($"Order with id: {request.orderId} successfully cancelled");
    }
}