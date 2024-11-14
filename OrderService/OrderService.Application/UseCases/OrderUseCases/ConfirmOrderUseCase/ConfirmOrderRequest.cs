using MediatR;

namespace OrderService.Application.UseCases.OrderUseCases.ConfirmOrderUseCase;

public sealed record ConfirmOrderRequest(string orderId) : IRequest { }
