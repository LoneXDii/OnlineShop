using MediatR;
using OrderService.Application.DTO;

namespace OrderService.Application.UseCases.CartUseCases.AddProductToCartUseCase;

public sealed record AddProductToCartRequest(AddProductToCartDTO product) : IRequest { }