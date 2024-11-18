using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Application.UseCases.OrderUseCases.CancelOrderUseCase;

internal class CancelOrderRequestHandler(IOrderRepository dbService, IProductService productService)
    : IRequestHandler<CancelOrderRequest>
{
    public async Task Handle(CancelOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await dbService.GetByIdAsync(request.orderId);

        if (order is null)
        {
            throw new NotFoundException("No such order");
        }

        if (request.userId is not null)
        {
            if (order.UserId != request.userId)
            {
                throw new AccessDeniedException("You dont have access to this order");
            }
        }

        if(order.OrderStatus == OrderStatuses.Completed)
        {
            throw new BadRequestException("Cannot cancel completed order");
        }

        order.OrderStatus = OrderStatuses.Cancelled;

        await dbService.UpdateAsync(order);

        await productService.ReturnProductsAsync(order.Products);
    }
}