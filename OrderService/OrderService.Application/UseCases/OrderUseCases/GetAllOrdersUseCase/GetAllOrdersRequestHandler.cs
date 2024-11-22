using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Models;
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

        var data = await dbService.ListWithPaginationAsync(request.pageNo, pageSize, cancellationToken);

        if (data.CurrentPage > data.TotalPages)
        {
            throw new NotFoundException("No such page");
        }

        var retData = mapper.Map<PaginatedListModel<OrderDTO>>(data);

        return retData;
    }
}
