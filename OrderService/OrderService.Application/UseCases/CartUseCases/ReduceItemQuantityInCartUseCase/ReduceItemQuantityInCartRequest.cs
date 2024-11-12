using MediatR;
using OrderService.Application.DTO;

namespace OrderService.Application.UseCases.CartUseCases.ReduceItemQuantityInCartUseCase;

public sealed record ReduceItemsInCartRequest(CartProductDTO product) : IRequest { }