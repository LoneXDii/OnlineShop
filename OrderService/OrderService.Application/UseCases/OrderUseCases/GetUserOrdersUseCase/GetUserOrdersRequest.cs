using MediatR;
using OrderService.Application.DTO;
using OrderService.Application.Models;

namespace OrderService.Application.UseCases.OrderUseCases.GetUserOrdersUseCase;

public sealed record GetUserOrdersRequest(string userId, int pageNo = 1, int pageSize = 10) : IRequest<PaginatedListModel<OrderDTO>> { }