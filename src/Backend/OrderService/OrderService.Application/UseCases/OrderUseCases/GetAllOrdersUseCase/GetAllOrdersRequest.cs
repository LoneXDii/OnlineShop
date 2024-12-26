using MediatR;
using OrderService.Application.DTO;
using OrderService.Application.Models;

namespace OrderService.Application.UseCases.OrderUseCases.GetAllOrdersUseCase;

public sealed record GetAllOrdersRequest(int PageNo = 1, int PageSize = 10) : IRequest<PaginatedListModel<OrderDTO>> { }