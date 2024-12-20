using MediatR;
using OrderService.Application.DTO;

namespace OrderService.Application.UseCases.CartUseCases.GetCartUseCase;

public sealed record GetCartRequest() : IRequest<CartDTO> { }