using MediatR;

namespace OrderService.Application.UseCases.OrderUseCases.CreateOrderUseCase;

public sealed record CreateOrderRequest(string userId) : IRequest { }