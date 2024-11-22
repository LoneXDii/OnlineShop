using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Application.Models;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Configuration;

namespace OrderService.Application.UseCases.OrderUseCases.GetAllOrdersUseCase;

internal class GetAllOrdersRequestHandler(IOrderRepository dbService, IMapper mapper, IOptions<PaginationSettings> paginationOptions)
    : IRequestHandler<GetAllOrdersRequest, PaginatedListModel<OrderDTO>>
{
    public async Task<PaginatedListModel<OrderDTO>> Handle(GetAllOrdersRequest request, CancellationToken cancellationToken)
    {
        var maxPageSize = paginationOptions.Value.MaxPageSize;

        var pageSize = request.pageSize > maxPageSize
            ? maxPageSize
            : request.pageSize;

        var items = await dbService.ListWithPaginationAsync(request.pageNo, pageSize, cancellationToken);

        var itemsCount = await dbService.CountAsync(cancellationToken);

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
