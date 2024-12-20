using MediatR;

namespace OrderService.Application.UseCases.PaymentUseCases.ConfirmPaymentUseCase;

public sealed record ConfirmPaymentRequest(string json, string signature) : IRequest { }