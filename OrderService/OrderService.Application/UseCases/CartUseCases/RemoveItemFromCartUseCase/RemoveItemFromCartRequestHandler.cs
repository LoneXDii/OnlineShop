using MediatR;
using OrderService.Domain.Abstractions.Cart;

namespace OrderService.Application.UseCases.CartUseCases.RemoveItemFromCartUseCase;

internal class RemoveItemFromCartRequestHandler(Cart cart) : IRequestHandler<RemoveItemFromCartRequest>
{
	public async Task Handle(RemoveItemFromCartRequest request, CancellationToken cancellationToken)
	{
		cart.RemoveItems(request.itemId);
	}
}
