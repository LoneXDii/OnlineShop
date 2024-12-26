using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.OrderUseCases.GetOrderByIdUseCase;

internal class GetOrderByIdRequestHandler(IOrderRepository orderRepository, IMapper mapper,
    ILogger<GetOrderByIdRequestHandler> logger)
    : IRequestHandler<GetOrderByIdRequest, OrderDTO>
{
    public async Task<OrderDTO> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.orderId, cancellationToken);

        if (order is null)
        {
            logger.LogError($"Order with id: {request.orderId} not found");

            throw new NotFoundException("No such order"); 
        }

        if (request.userId is not null)
        {
            if(order.UserId != request.userId)
            {
                logger.LogError($"User with id: {request.userId} has no access to order with id: {request.orderId}");

                throw new AccessDeniedException("You dont have access to this order");
            }
        }

        return mapper.Map<OrderDTO>(order);
    }
}
