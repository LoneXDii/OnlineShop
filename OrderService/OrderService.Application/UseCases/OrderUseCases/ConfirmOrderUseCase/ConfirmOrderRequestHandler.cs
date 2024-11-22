using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Application.UseCases.OrderUseCases.ConfirmOrderUseCase;

internal class ConfirmOrderRequestHandler(IOrderRepository dbService)
    : IRequestHandler<ConfirmOrderRequest>
{
    public async Task Handle(ConfirmOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await dbService.GetByIdAsync(request.orderId, cancellationToken);

        if (order is null)
        {
            throw new NotFoundException("No such order");
        }

        if (order.OrderStatus != OrderStatuses.Created)
        {
            throw new BadRequestException("Order confirmation error, invalid order status");
        }

        order.OrderStatus = OrderStatuses.Confirmed;

        await dbService.UpdateAsync(order, cancellationToken);
    }
}
