using MediatR;

namespace OrderService.Application.UseCases.OrderUseCases.CompleteOrderUseCase;

public sealed record CompleteOrderRequest(string orderId) : IRequest { }
