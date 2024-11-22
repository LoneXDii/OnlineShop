using MediatR;
using OrderService.Application.DTO;
using OrderService.Application.Models;

namespace OrderService.Application.UseCases.OrderUseCases.GetAllOrdersUseCase;

public sealed record GetAllOrdersRequest(int pageNo = 1, int pageSize = 10) : IRequest<PaginatedListModel<OrderDTO>> { }