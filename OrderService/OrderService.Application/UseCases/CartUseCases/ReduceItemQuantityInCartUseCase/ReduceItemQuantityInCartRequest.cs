using MediatR;
using OrderService.Application.DTO;

namespace OrderService.Application.UseCases.CartUseCases.ReduceItemQuantityInCartUseCase;

public sealed record ReduceItemsInCartRequest(int ProductId, int Quantity) : IRequest { }