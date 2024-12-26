using MediatR;

namespace OrderService.Application.UseCases.OrderUseCases.CancelOrderUseCase;

public sealed record CancelOrderRequest(string orderId, string? userId = null) : IRequest { }