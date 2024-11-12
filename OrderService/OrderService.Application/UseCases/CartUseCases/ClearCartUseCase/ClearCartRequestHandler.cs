using MediatR;
using OrderService.Domain.Abstractions.Cart;

namespace OrderService.Application.UseCases.CartUseCases.ClearCartUseCase;

internal class ClearCartRequestHandler(Cart cart) : IRequestHandler<ClearCartRequest>
{
	public async Task Handle(ClearCartRequest request, CancellationToken cancellationToken)
	{
		cart.ClearAll();
	}
}
