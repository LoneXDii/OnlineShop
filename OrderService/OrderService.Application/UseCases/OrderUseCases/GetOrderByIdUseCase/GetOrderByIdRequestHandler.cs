using AutoMapper;
using MediatR;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.OrderUseCases.GetOrderByIdUseCase;

internal class GetOrderByIdRequestHandler(IDbService dbService, IMapper mapper)
    : IRequestHandler<GetOrderByIdRequest, GetOrderDTO>
{
    public async Task<GetOrderDTO> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
    {
        var order = await dbService.GetOrderByIdAsync(request.orderId);

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

        var returnOrder = mapper.Map<GetOrderDTO>(order);

        return returnOrder;
    }
}
