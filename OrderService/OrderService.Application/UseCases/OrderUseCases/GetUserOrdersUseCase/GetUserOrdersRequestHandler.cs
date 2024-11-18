using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Models;

namespace OrderService.Application.UseCases.OrderUseCases.GetUserOrdersUseCase;

internal class GetUserOrdersRequestHandler(IOrderRepository dbService, IMapper mapper, IConfiguration configuration)
    : IRequestHandler<GetUserOrdersRequest, PaginatedListModel<GetOrderDTO>>
{
    public async Task<PaginatedListModel<GetOrderDTO>> Handle(GetUserOrdersRequest request, CancellationToken cancellationToken)
    {
        var maxPageSize = Convert.ToInt32(configuration["Pagination:MaxPageSize"]);
        
        var pageSize = request.pageSize > maxPageSize 
            ? maxPageSize 
            : request.pageSize;

        var data = await dbService.ListWithPaginationAsync(request.pageNo, pageSize, order => order.UserId == request.userId);

        if(data.CurrentPage > data.TotalPages)
        {
            throw new PaginationException("No such page");
        }

        var retData = mapper.Map<PaginatedListModel<GetOrderDTO>>(data);

        return retData;
    }
}
