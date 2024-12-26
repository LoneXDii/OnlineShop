using MediatR;
using OrderService.Application.DTO;
using OrderService.Application.Models;

namespace OrderService.Application.UseCases.OrderUseCases.GetUserOrdersUseCase;

public sealed class GetUserOrdersRequest() : IRequest<PaginatedListModel<OrderDTO>> 
{
    public string UserId { get; set; } = "";
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}