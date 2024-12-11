using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Application.Models;
using OrderService.Domain.Abstractions.Data;
using OrderService.Application.Settings;

namespace OrderService.Application.UseCases.OrderUseCases.GetUserOrdersUseCase;

internal class GetUserOrdersRequestHandler(IOrderRepository orderRepository, IMapper mapper, IOptions<PaginationSettings> paginationOptions)
    : IRequestHandler<GetUserOrdersRequest, PaginatedListModel<OrderDTO>>
{
    public async Task<PaginatedListModel<OrderDTO>> Handle(GetUserOrdersRequest request, CancellationToken cancellationToken)
    {
        var maxPageSize = paginationOptions.Value.MaxPageSize;
        
        var pageSize = request.PageSize > maxPageSize 
            ? maxPageSize 
            : request.PageSize;

        var items = await orderRepository.ListWithPaginationAsync(request.PageNo, pageSize, cancellationToken, order => order.UserId == request.UserId);

        var itemsCount = await orderRepository.CountAsync(cancellationToken, order => order.UserId == request.UserId);

        var totalPages = (int)Math.Ceiling(itemsCount / (double)pageSize);

        if (request.PageNo > totalPages)
        {
            throw new NotFoundException("No such page");
        }

        var data = new PaginatedListModel<OrderDTO>
        {
            Items = mapper.Map<List<OrderDTO>>(items),
            CurrentPage = request.PageNo,
            TotalPages = totalPages
        };

        return data;
    }
}
