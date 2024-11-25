using MediatR;
using OrderService.Domain.Abstractions.Cart;

namespace OrderService.Application.UseCases.CartUseCases.RemoveItemFromCartUseCase;

internal class RemoveItemFromCartRequestHandler(ICart cart) : IRequestHandler<RemoveItemFromCartRequest>
{
    public Task Handle(RemoveItemFromCartRequest request, CancellationToken cancellationToken)
    {
        cart.RemoveItems(request.itemId);

        return Task.CompletedTask;
    }
}
