using MediatR;

namespace OrderService.Application.UseCases.PaymentUseCases.PayOrderUseCase;

public sealed record PayOrderRequest(string orderId, string userId, string stribeId) : IRequest<string> { }