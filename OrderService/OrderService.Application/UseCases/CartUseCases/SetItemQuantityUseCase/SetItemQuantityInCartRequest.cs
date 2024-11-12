using MediatR;
using OrderService.Application.DTO;

namespace OrderService.Application.UseCases.CartUseCases.SetItemQuantityInCartUseCase;

public sealed record SetItemQuantityInCartRequest(CartProductDTO product) : IRequest { }