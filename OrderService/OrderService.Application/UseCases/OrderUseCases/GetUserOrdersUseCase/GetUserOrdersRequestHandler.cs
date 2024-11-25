using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Application.Models;
using OrderService.Domain.Abstractions.Data;
using OrderService.Application.Settings;

namespace OrderService.Application.UseCases.OrderUseCases.GetUserOrdersUseCase;

internal class GetUserOrdersRequestHandler(IOrderRepository dbService, IMapper mapper, IOptions<PaginationSettings> paginationOptions)
    : IRequestHandler<GetUserOrdersRequest, PaginatedListModel<OrderDTO>>
{
    public async Task<PaginatedListModel<OrderDTO>> Handle(GetUserOrdersRequest request, CancellationToken cancellationToken)
    {
        var maxPageSize = paginationOptions.Value.MaxPageSize;
        
        var pageSize = request.pageSize > maxPageSize 
            ? maxPageSize 
            : request.pageSize;

        var items = await dbService.ListWithPaginationAsync(request.pageNo, pageSize, cancellationToken, order => order.UserId == request.userId);

        var itemsCount = await dbService.CountAsync(cancellationToken, order => order.UserId == request.userId);

        //Move to validator
        var totalPages = (int)Math.Ceiling(itemsCount / (double)pageSize);

        if (request.pageNo > totalPages)
        {
            throw new NotFoundException("No such page");
        }

        var data = new PaginatedListModel<OrderDTO>
        {
            Items = mapper.Map<List<OrderDTO>>(items),
            CurrentPage = request.pageNo,
            TotalPages = totalPages
        };

        return data;
    }
}
