using MediatR;

namespace OrderService.Application.UseCases.CartUseCases.ClearCartUseCase;

public sealed record ClearCartRequest() : IRequest {}