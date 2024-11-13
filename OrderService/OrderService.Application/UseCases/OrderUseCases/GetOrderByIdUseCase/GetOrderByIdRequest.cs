using MediatR;
using OrderService.Application.DTO;

namespace OrderService.Application.UseCases.OrderUseCases.GetOrderByIdUseCase;

public sealed record GetOrderByIdRequest(string orderId, string? userId = null) : IRequest<GetOrderDTO> { }