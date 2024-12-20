using AutoMapper;
using MediatR;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.OrderUseCases.GetOrderByIdUseCase;

internal class GetOrderByIdRequestHandler(IOrderRepository orderRepository, IMapper mapper)
    : IRequestHandler<GetOrderByIdRequest, OrderDTO>
{
    public async Task<OrderDTO> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.orderId, cancellationToken);

        if (order is null)
        {
            throw new NotFoundException("No such order");
        }

        if (request.userId is not null)
        {
            if(order.UserId != request.userId)
            {
                throw new AccessDeniedException("You dont have access to this order");
            }
        }

        return mapper.Map<OrderDTO>(order);
    }
}
