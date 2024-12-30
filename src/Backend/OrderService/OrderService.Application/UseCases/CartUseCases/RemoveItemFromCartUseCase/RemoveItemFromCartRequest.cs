using MediatR;

namespace OrderService.Application.UseCases.CartUseCases.RemoveItemFromCartUseCase;

public sealed record RemoveItemFromCartRequest(int itemId) : IRequest { }