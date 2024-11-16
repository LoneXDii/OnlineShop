using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Application.UseCases.OrderUseCases.CompleteOrderUseCase;

internal class CompleteOrderRequestHandler(IDbService dbService)
    : IRequestHandler<CompleteOrderRequest>
{
    public async Task Handle(CompleteOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await dbService.GetOrderByIdAsync(request.orderId);

        if (order is null)
        {
            throw new NotFoundException("No such order");
        }

        if (order.OrderStatus != OrderStatuses.Confirmed)
        {
            throw new BadRequestException("Can complete only confirmed orders");
        }

        if (order.PaymentStatus == PaymentStatuses.NotPaid)
        {
            throw new BadRequestException("Can't complete unpaid order'");
        }

        order.OrderStatus = OrderStatuses.Completed;

        await dbService.UpdateOrderAsync(order);
    }
}
